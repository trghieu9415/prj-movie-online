using Mv.Domain.Enums;

namespace Mv.Application.Ports.Gateway;

public interface IGatewayFactory {
  IPaymentGateway CreatePaymentGateway(PaymentMethod method);
}
