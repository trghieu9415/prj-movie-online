using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Gateway;
using Mv.Infrastructure.Adapters.Gateway;
using Mv.Infrastructure.Adapters.Gateway.Transaction;
using Mv.Infrastructure.Services;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Extensions;

public static class ExternalServiceExtensions {
  public static IServiceCollection AddExternalServices(this IServiceCollection services) {
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IStorageService, LocalStorageService>();

    // Transactions
    services.AddHttpClient();
    services.AddScoped<IGatewayFactory, GatewayFactory>();
    services.AddKeyedScoped<IPaymentGateway, StripeGateway>(PaymentMethod.Stripe);
    services.AddKeyedScoped<IPaymentGateway, PaypalGateway>(PaymentMethod.Paypal);

    services.AddAutoMapper(_ => {}, typeof(InfrastructureConfiguration).Assembly);
    return services;
  }
}
