using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Gateway;

namespace Mv.Infrastructure.Adapters.Gateway;

public class GatewayFactory(IServiceProvider serviceProvider) : IGatewayFactory {
  public IPaymentGateway CreatePaymentGateway(PaymentMethod? method) {
    return serviceProvider.GetRequiredKeyedService<IPaymentGateway>(method);
  }
}
