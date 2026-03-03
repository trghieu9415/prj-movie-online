using Domain.Enums;
using Mv.Application.Ports.Gateway;

namespace Mv.Infrastructure.Adapters.Gateway;

public class GatewayFactory : IGatewayFactory {
  public IPaymentGateway CreatePaymentGateway(PaymentMethod method) {
    throw new NotImplementedException();
  }
}
