using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Abstractions;
using Mv.Application.Repositories;
using Mv.Application.Repositories.Read;
using Mv.Infrastructure.Persistence;
using Mv.Infrastructure.Persistence.Repositories;
using Mv.Infrastructure.Persistence.Repositories.Read;
using Mv.Infrastructure.Seeding;
using Mv.Infrastructure.Seeding.Seeders;
using Npgsql;

namespace Mv.Infrastructure.Extensions;

public static class PersistenceExtensions {
  public static IServiceCollection AddPostgresPersistence(this IServiceCollection services, IConfiguration config) {
    // Connection & DbContext
    var connectionString = config.GetConnectionString("DefaultConnection");
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder.EnableDynamicJson();

    services.AddSingleton(dataSourceBuilder.Build());
    services.AddDbContext<AppDbContext>((serviceProvider, options) => {
      var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
      options.UseNpgsql(dataSource, builder =>
        builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
      );
      options.ConfigureWarnings(warning =>
        warning.Ignore(RelationalEventId.PendingModelChangesWarning)
      );
    });

    services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());

    // Repositories
    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<,>), typeof(EfReadRepository<,>));
    services.AddScoped<IPlanReadRepository, PlanReadRepository>();
    services.AddScoped<IListingReadRepository, ListingReadRepository>();
    services.AddScoped<IShowtimeReadRepository, ShowtimeReadRepository>();

    // Seeding
    services.AddScoped<DbInitializer>();
    services.AddScoped<ISeeder, AdminSeeder>();
    services.AddScoped<ISeeder, UserSeeder>();
    services.AddScoped<ISeeder, MovieSeeder>();
    services.AddScoped<ISeeder, PlanSeeder>();
    services.AddScoped<ISeeder, ListingSeeder>();

    return services;
  }
}
