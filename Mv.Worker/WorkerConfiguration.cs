using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Messaging;
using Mv.Worker.Adapters.Messaging;
using Mv.Worker.Extensions;

namespace Mv.Worker;

public static class WorkerConfiguration {
  public static IServiceCollection AddWorker(this IServiceCollection services, IConfiguration config) {
    services
      .AddQuartzInfrastructure(config)
      .AddCustomMassTransit(config)
      .AddFireAndForget();

    return services;
  }

  private static IServiceCollection AddFireAndForget(this IServiceCollection services) {
    services.AddSingleton<IBackgroundTaskQueue>(new BackgroundTaskQueue(100));
    services.AddHostedService<QueuedHostedService>();
    return services;
  }
}
