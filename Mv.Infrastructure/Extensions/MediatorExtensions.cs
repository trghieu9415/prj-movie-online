using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application;
using Mv.Application.Behaviors;
using Mv.Application.Ports.Logging;
using Mv.Infrastructure.Adapters.Logging;

namespace Mv.Infrastructure.Extensions;

public static class MediatorExtensions {
  public static IServiceCollection AddMediatorPipeline(this IServiceCollection services) {
    var applicationAssembly = typeof(IApplicationMarker).Assembly;

    services.AddMediatR(cfg => {
      cfg.RegisterServicesFromAssembly(applicationAssembly);

      cfg.AddOpenBehavior(typeof(LockBehavior<,>));
      cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
      cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
    });

    services.AddValidatorsFromAssembly(applicationAssembly);
    services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
    return services;
  }
}
