using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mv.Application.Ports.Realtime;
using Mv.Application.Ports.Security;
using Mv.Infrastructure.Configs.Options;
using Mv.Presentation.Adapters.Realtime;
using Mv.Presentation.Adapters.Security;

namespace Mv.Presentation.Extensions;

public static class PresentationExtensions {
  public static IServiceCollection AddPresentationInfrastructure(this IServiceCollection services) {
    services
      .AddRealtime()
      .AddApiAuthentication();
    return services;
  }

  private static IServiceCollection AddRealtime(this IServiceCollection services) {
    services.AddSignalR();
    services.AddScoped<IShowtimeNotifier, ShowtimeNotifier>();
    services.AddScoped<IUserNotifier, UserNotifier>();
    return services;
  }

  public static IServiceCollection AddApiAuthentication(this IServiceCollection services) {
    services.AddScoped<ICurrentUser, CurrentUser>();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer();

    services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
      .Configure<IOptions<JwtOptions>>((bearerOptions, jwtOptionsRef) => {
          var jwtOptions = jwtOptionsRef.Value;

          bearerOptions.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
          };

          bearerOptions.Events = new JwtBearerEvents {
            OnTokenValidated = async context => {
              var auth = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

              var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
              var tokenStamp = context.Principal?.FindFirstValue("security_stamp");

              if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(tokenStamp)) {
                context.Fail("Token thiếu thông tin bảo mật.");
                return;
              }

              if (!Guid.TryParse(userId, out var userGuid)) {
                context.Fail("Định danh người dùng không hợp lệ.");
                return;
              }

              var stampValidated = await auth.ValidateSecurityStampAsync(
                userGuid, tokenStamp, context.HttpContext.RequestAborted
              );
              if (!stampValidated) {
                context.Fail("Phiên làm việc đã hết hạn hoặc mật khẩu đã thay đổi.");
              }
            },

            OnMessageReceived = context => {
              var accessToken = context.Request.Query["access_token"];
              var path = context.HttpContext.Request.Path;
              if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")) {
                context.Token = accessToken;
              }

              return Task.CompletedTask;
            }
          };
        }
      );

    return services;
  }
}
