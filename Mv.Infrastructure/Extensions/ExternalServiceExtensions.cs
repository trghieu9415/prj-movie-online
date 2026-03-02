using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Notification;
using Mv.Application.Ports.Storage;
using Mv.Infrastructure.Adapters.Notification;
using Mv.Infrastructure.Adapters.Storage;

namespace Mv.Infrastructure.Extensions;

public static class ExternalServiceExtensions {
  public static IServiceCollection AddExternalServices(this IServiceCollection services) {
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IBinaryStorage, LocalBinaryStorage>();

    services.AddAutoMapper(_ => {}, typeof(InfrastructureConfiguration).Assembly);
    return services;
  }
}
