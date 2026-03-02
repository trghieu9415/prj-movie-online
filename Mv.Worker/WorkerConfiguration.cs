using L3.Worker.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mv.Worker.Extensions;

namespace L3.Worker;

public static class WorkerConfiguration {
  public static IServiceCollection AddWorker(this IServiceCollection services, IConfiguration config) {
    services
      .AddQuartzInfrastructure(config)
      .AddCustomMassTransit();

    return services;
  }
}
