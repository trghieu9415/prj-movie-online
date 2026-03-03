using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mv.Infrastructure.Extensions;

namespace Mv.Infrastructure;

public static class InfrastructureConfiguration {
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config) {
    services
      .AddConfigurationOptions(config)
      .AddPostgresPersistence(config)
      .AddIdentityInfrastructure()
      .AddDistributedInfrastructure()
      .AddMediatorPipeline()
      .AddExternalServices();


    return services;
  }
}
