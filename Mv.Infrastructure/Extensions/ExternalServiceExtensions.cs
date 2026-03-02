using Microsoft.Extensions.DependencyInjection;
using Mv.Infrastructure.Services;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Extensions;

public static class ExternalServiceExtensions {
  public static IServiceCollection AddExternalServices(this IServiceCollection services) {
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IStorageService, LocalStorageService>();

    services.AddAutoMapper(_ => {}, typeof(InfrastructureConfiguration).Assembly);
    return services;
  }
}
