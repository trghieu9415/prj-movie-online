using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Gateway;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;
using Mv.Domain.Entities;
using Mv.Domain.Enums;

namespace Mv.Application.UseCases.Booking.CreatePayment;

public class CreatePaymentHandler(
  IRepository<Payment> paymentRepository,
  IRepository<Order> orderRepository,
  ICurrentUser currentUser,
  IGatewayFactory gatewayFactory
) : IRequestHandler<CreatePaymentCommand, CreatePaymentResult> {
  public async Task<CreatePaymentResult> Handle(CreatePaymentCommand request, CancellationToken ct) {
    var order =
      await orderRepository.GetByIdAsync(request.OrderId, ct)
      ?? throw new WorkflowException("Đơn hàng không tồn tại", 404);

    if (currentUser.Id != order.CustomerId) {
      throw new WorkflowException("Đơn hàng không thuộc về người dùng hiện tại", 403);
    }

    if (order.Status != OrderStatus.Pending) {
      throw new WorkflowException("Chỉ có thể thanh toán đơn hàng đang ở trạng thái chờ", 429);
    }


    var paymentGateway = gatewayFactory.CreatePaymentGateway(request.Method);

    var payment = await paymentRepository.GetFirstAsync(x => x.OrderId == order.Id, ct);
    if (payment != null) {
      return new CreatePaymentResult(payment.Id, payment.PaymentUrl!);
    }

    payment = Payment.Create(order.Id, order.TotalPrice, request.Method);
    var paymentUrl = await paymentGateway.CreatePaymentUrl(payment, order, ct);
    payment.SetPaymentUrl(paymentUrl);

    await paymentRepository.CreateAsync(payment, ct);
    return new CreatePaymentResult(payment.Id, paymentUrl);
  }
}
