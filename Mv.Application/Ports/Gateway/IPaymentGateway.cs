using Domain.Entities;

namespace Mv.Application.Ports.Gateway;

public interface IPaymentGateway {
  string CreatePaymentUrl(Payment payment, Order order);
  (bool isSucceed, string transactionId) VerifyPayment(GatewayPayload payload);
  bool RefundPayment(Payment payment);
  public GatewayPayload ToGatewayPayload(object data);
}

public abstract record GatewayPayload;
