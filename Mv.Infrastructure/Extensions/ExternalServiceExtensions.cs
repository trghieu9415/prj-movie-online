using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Mv.Application.Constants;
using Mv.Application.Ports.Gateway;
using Mv.Domain.Enums;
using Mv.Infrastructure.Adapters.Gateway;
using Mv.Infrastructure.Adapters.Gateway.Transaction;
using Mv.Infrastructure.Services;
using Mv.Infrastructure.Services.Abstractions;
using Polly;

namespace Mv.Infrastructure.Extensions;

public static class ExternalServiceExtensions {
  public static IServiceCollection AddExternalServices(this IServiceCollection services) {
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IStorageService, S3StorageService>();
    services.AddScoped<IImageTracker, ImageTracker>();

    services.AddHttpClient(HttpClientNames.Paypal)
      .AddStandardResilienceHandler(ConfigureExternalServicesResilience());

    // Transactions
    services.AddScoped<IGatewayFactory, GatewayFactory>();
    services.AddKeyedScoped<IPaymentGateway, StripeGateway>(PaymentMethod.Stripe);
    services.AddKeyedScoped<IPaymentGateway, PaypalGateway>(PaymentMethod.Paypal);

    services.AddAutoMapper(_ => {}, typeof(InfrastructureConfiguration).Assembly);
    return services;
  }

  private static Action<HttpStandardResilienceOptions> ConfigureExternalServicesResilience() {
    return options => {
      // Retry Policy
      options.Retry.MaxRetryAttempts = 3;
      options.Retry.Delay = TimeSpan.FromSeconds(2);
      options.Retry.BackoffType = DelayBackoffType.Exponential;

      // Circuit Breaker
      options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
      options.CircuitBreaker.FailureRatio = 0.5;
      options.CircuitBreaker.MinimumThroughput = 5;
      options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
      options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(45);
    };
  }
}
