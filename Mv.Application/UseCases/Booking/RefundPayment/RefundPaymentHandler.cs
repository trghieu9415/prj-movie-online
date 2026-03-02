using Domain.Entities;
using MediatR;
using Mv.Application.Ports.Gateway;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Booking.RefundPayment;

public class RefundPaymentHandler(
  IRepository<Payment> paymentRepository,
  IGatewayFactory gatewayFactory
) : IRequestHandler<RefundPaymentCommand, bool> {
  public async Task<bool> Handle(RefundPaymentCommand request, CancellationToken ct) {
    var payment = await paymentRepository.GetByIdAsync(request.Id, ct);
    if (payment == null) {
      return false;
    }

    var paymentGateway = gatewayFactory.CreatePaymentGateway(payment.Method);
    var refunded = paymentGateway.RefundPayment(payment);
    payment.Refund();
    await paymentRepository.UpdateAsync(payment, ct);
    return refunded;
  }
}
