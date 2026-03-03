using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mv.Application.Ports.Messaging;
using Mv.Infrastructure.Configs.Options;
using Mv.Infrastructure.Persistence;
using Mv.Worker.Adapters.Messaging;

namespace Mv.Worker.Extensions;

public static class MassTransitExtensions {
  public static IServiceCollection AddCustomMassTransit(this IServiceCollection services) {
    services.AddMassTransit(x => {
      x.AddConsumers(typeof(WorkerConfiguration).Assembly);

      x.AddQuartzConsumers();
      x.AddPublishMessageScheduler();

      x.AddEntityFrameworkOutbox<AppDbContext>(o => {
        o.UsePostgres();
        o.UseBusOutbox();
        o.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);
      });

      x.UsingRabbitMq((context, cfg) => {
        var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        cfg.Host(options.Host, options.VirtualHost, h => {
          h.Username(options.Username);
          h.Password(options.Password);
        });

        cfg.UsePublishMessageScheduler();
        cfg.UseMessageRetry(r =>
          r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2))
        );
        cfg.UseCircuitBreaker(cb => {
          cb.TrackingPeriod = TimeSpan.FromMinutes(1);
          cb.TripThreshold = 15;
          cb.ActiveThreshold = 10;
          cb.ResetInterval = TimeSpan.FromMinutes(5);
        });

        cfg.ConfigureEndpoints(context);
      });
    });

    services.AddScoped<IEventDispatcher, MassTransitEventDispatcher>();
    return services;
  }
}
