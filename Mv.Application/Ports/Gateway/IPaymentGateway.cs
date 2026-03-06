using Domain.Entities;

namespace Mv.Application.Ports.Gateway;

public interface IPaymentGateway {
  Task<string> CreatePaymentUrl(Payment payment, Order order, CancellationToken ct = default);
  Task<(bool isSucceed, string transactionId)> VerifyPayment(GatewayPayload payload, CancellationToken ct = default);
  Task<bool> RefundPayment(Payment payment, CancellationToken ct = default);
  public GatewayPayload ToGatewayPayload(object data);
}

public abstract record GatewayPayload;
