using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mv.Application.Ports.Cache;
using Mv.Application.Ports.Concurrency;
using Mv.Infrastructure.Adapters.Cache;
using Mv.Infrastructure.Adapters.Concurrency;
using Mv.Infrastructure.Configs.Options;
using Mv.Infrastructure.Services;
using Mv.Infrastructure.Services.Abstractions;
using StackExchange.Redis;

namespace Mv.Infrastructure.Extensions;

public static class DistributedExtensions {
  public static IServiceCollection
    AddDistributedInfrastructure(this IServiceCollection services) {
    // Redis Cache
    services.AddOptions<RedisCacheOptions>()
      .Configure<IOptions<RedisOptions>>((cacheOptions, redisOptionsRef) => {
        var redisOptions = redisOptionsRef.Value;
        cacheOptions.Configuration = redisOptions.Configuration;
        cacheOptions.InstanceName = redisOptions.InstanceName;
      });
    services.AddStackExchangeRedisCache(_ => {});
    services.AddScoped<ICacheService, RedisCacheService>();
    services.AddScoped<IBusinessCache, BusinessCache>();

    // Redis Connection (Multiplexer for Locking)
    services.AddSingleton<IConnectionMultiplexer>(serviceProvider => {
      var redisOptions = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;
      return ConnectionMultiplexer.Connect(redisOptions.Configuration);
    });

    // Distributed Lock (Medallion)
    services.AddSingleton<IDistributedLockProvider>(serviceProvider => {
      var connection = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
      return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
    });
    services.AddScoped<IDistributedLockService, RedisLockService>();

    return services;
  }
}
