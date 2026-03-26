using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Gateway;
using Mv.Domain.Enums;

namespace Mv.Infrastructure.Adapters.Gateway;

public class GatewayFactory(IServiceProvider serviceProvider) : IGatewayFactory {
  public IPaymentGateway CreatePaymentGateway(PaymentMethod method) {
    return serviceProvider.GetRequiredKeyedService<IPaymentGateway>(method);
  }
}
