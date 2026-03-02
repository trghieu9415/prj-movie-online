using Domain.Entities;
using MediatR;
using Mv.Application.Ports.Gateway;
using Mv.Application.Ports.Repositories;

namespace Mv.Application.UseCases.Booking.ProcessPayment;

public class ProcessPaymentHandler(
  IRepository<Payment> paymentRepository,
  IGatewayFactory gatewayFactory
) : IRequestHandler<ProcessPaymentCommand, bool> {
  public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken ct) {
    var payment = await paymentRepository.GetByIdAsync(request.Id, ct);
    if (payment == null) {
      return false;
    }

    var paymentGateway = gatewayFactory.CreatePaymentGateway(payment.Method);
    var payload = paymentGateway.ToPaymentPayload(request.Payload);
    var (isSucceed, transactionId) = paymentGateway.VerifyPayment(payload);

    if (isSucceed) {
      payment.MarkAsSucceeded(transactionId);
    } else {
      payment.MarkAsFailed();
    }

    return true;
  }
}
