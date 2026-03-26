using MediatR;
using Mv.Application.Ports.Gateway;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Booking.ProcessPayment;

public class ProcessPaymentHandler(
  IRepository<Payment> paymentRepository,
  IGatewayFactory gatewayFactory,
  ICurrentUser currentUser
) : IRequestHandler<ProcessPaymentCommand, bool> {
  public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken ct) {
    var payment = await paymentRepository.GetByIdAsync(request.Id, ct);
    if (payment == null) {
      return false;
    }

    var paymentGateway = gatewayFactory.CreatePaymentGateway(payment.Method);
    var gatewayPayload = paymentGateway.ToGatewayPayload(request.Payload);
    var (isSucceed, transactionId) = await paymentGateway.VerifyPayment(gatewayPayload, ct);

    if (isSucceed) {
      payment.MarkAsSucceeded(currentUser.Id, transactionId);
    } else {
      payment.MarkAsFailed();
    }

    return true;
  }
}
