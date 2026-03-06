using Domain.Entities;
using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Gateway;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;

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

    var payment = Payment.Create(order.Id, order.TotalPrice, request.Method);
    var paymentGateway = gatewayFactory.CreatePaymentGateway(request.Method);
    var paymentUrl = await paymentGateway.CreatePaymentUrl(payment, order, ct);

    await paymentRepository.CreateAsync(payment, ct);
    return new CreatePaymentResult(payment.Id, paymentUrl);
  }
}
