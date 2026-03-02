using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Security;
using Mv.Infrastructure.Adapters.Security;
using Mv.Infrastructure.Identity;
using Mv.Infrastructure.Persistence;

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
    services.AddScoped<IJwtService, JwtService>();

    return services;
  }
}
