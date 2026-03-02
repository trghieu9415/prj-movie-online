using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Security;
using Mv.Infrastructure.Adapters.Security;
using Mv.Infrastructure.Persistence;
using Mv.Infrastructure.Persistence.Identity;
using Mv.Infrastructure.Services;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Extensions;

public static class IdentityExtensions {
  public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services) {
    // Identity Core Configuration
    services.AddIdentityCore<AppUser>(options => {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
      })
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();

    // Auth Services Implementation
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IUserService, UserService>();

    // Jwt Service
    services.AddScoped<IJwtService, JwtService>();

    return services;
  }
}
