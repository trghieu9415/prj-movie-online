using MediatR;
using Mv.Application.Ports.Gateway;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.System.RefundPayment;

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
    var refunded = await paymentGateway.RefundPayment(payment, ct);
    payment.Refund();
    await paymentRepository.UpdateAsync(payment, ct);
    return refunded;
  }
}
