using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Mv.Application.Exceptions;
using Mv.Application.Models;
using Mv.Application.Ports.Notification;
using Mv.Application.Ports.Security;
using Mv.Application.Ports.Storage;
using Mv.Infrastructure.Identity;
using Mv.Infrastructure.Options;

namespace Mv.Infrastructure.Adapters.Security;

public class AuthService(
  UserManager<AppUser> userManager,
  IJwtService jwtService,
  IEmailService emailService,
  ICacheStorage cache,
  JwtOptions jwtOptions
) : IAuthService {
  public async Task<AuthTokens> RegisterAsync(User user, string password, CancellationToken ct) {
    var existingUser = await userManager.FindByEmailAsync(user.Email);
    if (existingUser != null) {
      throw new WorkflowException("Email đã tồn tại trong hệ thống");
    }

    var appUser = new AppUser {
      Id = Guid.NewGuid(),
      Email = user.Email,
      UserName = user.Email,
      FullName = user.FullName,
      Role = UserRole.Customer,
      PhoneNumber = user.PhoneNumber
    };

    var result = await userManager.CreateAsync(appUser, password);
    if (!result.Succeeded) {
      throw new WorkflowException(result.Errors.First().Description);
    }

    var userModel = ToUserModel(appUser);
    return new AuthTokens(jwtService.GenerateAccessToken(userModel), jwtService.GenerateRefreshToken(userModel));
  }

  public async Task<AuthTokens> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword,
    CancellationToken ct) {
    var user = await userManager.FindByIdAsync(userId.ToString());
    if (user == null || user.IsDeleted) {
      throw new WorkflowException("Người dùng không tồn tại hoặc đã bị xóa", 404);
    }

    var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    if (!result.Succeeded) {
      var error = result.Errors.FirstOrDefault()?.Description ?? "Mật khẩu cũ không đúng";
      throw new WorkflowException(error);
    }

    var userModel = ToUserModel(user);
    return new AuthTokens(
      jwtService.GenerateAccessToken(userModel),
      jwtService.GenerateRefreshToken(userModel)
    );
  }

  public async Task RequestPasswordAsync(string email, CancellationToken ct) {
    var user = await userManager.FindByEmailAsync(email);
    if (user == null || user.IsDeleted || user.Role != UserRole.Customer) {
      return;
    }

    var token = await userManager.GeneratePasswordResetTokenAsync(user);
    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
    await emailService.SendResetPasswordEmailAsync(user.Email!, encodedToken, ct);
  }


  public async Task ResetPasswordAsync(string email, string token, string newPassword, CancellationToken ct) {
    var user = await userManager.FindByEmailAsync(email);
    if (user == null || user.IsDeleted) {
      throw new WorkflowException("Thông tin không hợp lệ", 404);
    }

    try {
      var decodedBytes = WebEncoders.Base64UrlDecode(token);
      var originalToken = Encoding.UTF8.GetString(decodedBytes);
      var result = await userManager.ResetPasswordAsync(user, originalToken, newPassword);
      if (!result.Succeeded) {
        var errorMsg = result.Errors.FirstOrDefault()?.Description;
        Console.WriteLine(errorMsg);
        throw new WorkflowException("Đổi mật khẩu thất bại");
      }

      await userManager.UpdateSecurityStampAsync(user);
    } catch {
      throw new WorkflowException("Mã xác nhận đã hết hạn hoặc không hợp lệ", 401);
    }
  }

  public async Task<AuthTokens> RefreshAsync(string refreshToken, CancellationToken ct) {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(jwtOptions.Secret!);

    tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(key),
      ValidateIssuer = true,
      ValidIssuer = jwtOptions.Issuer,
      ValidateAudience = true,
      ValidAudience = jwtOptions.Audience,
      ClockSkew = TimeSpan.Zero
    }, out var validatedToken);

    var jwtToken = (JwtSecurityToken)validatedToken;

    var tokenType = jwtToken.Claims.FirstOrDefault(x => x.Type == "token_type")?.Value;
    if (tokenType != "refresh") {
      throw new WorkflowException("Token không hợp lệ");
    }

    var isBlacklisted = await cache.IsBlacklistedAsync(jwtToken.Id, ct);
    if (isBlacklisted) {
      throw new WorkflowException("Phiên đăng nhập đã bị vô hiệu hóa", 401);
    }

    var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

    var user = await userManager.FindByIdAsync(userId);
    if (user == null || user.IsDeleted) {
      throw new WorkflowException("Tài khoản không hợp lệ", 401);
    }

    if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow) {
      throw new WorkflowException("Tài khoản đã bị khóa", 403);
    }

    var tokenStamp = jwtToken.Claims.FirstOrDefault(x => x.Type == "security_stamp")?.Value;
    if (tokenStamp != user.SecurityStamp) {
      throw new WorkflowException("Thông tin bảo mật đã thay đổi, vui lòng đăng nhập lại", 401);
    }

    await cache.BlacklistAsync(jwtToken.Id, jwtToken.ValidTo - DateTime.UtcNow, ct);

    var userModel = ToUserModel(user);
    return new AuthTokens(
      jwtService.GenerateAccessToken(userModel),
      jwtService.GenerateRefreshToken(userModel)
    );
  }

  public async Task LogoutAsync(string refreshToken, bool revokeAll, CancellationToken ct) {
    if (string.IsNullOrEmpty(refreshToken)) {
      return;
    }

    var tokenHandler = new JwtSecurityTokenHandler();
    try {
      var jwtToken = tokenHandler.ReadJwtToken(refreshToken);

      if (revokeAll) {
        var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var user = await userManager.FindByIdAsync(userId);
        if (user != null) {
          await userManager.UpdateSecurityStampAsync(user);
        }
      } else {
        var remainingTime = jwtToken.ValidTo - DateTime.UtcNow;

        if (remainingTime.TotalSeconds > 0) {
          await cache.BlacklistAsync(jwtToken.Id, remainingTime, ct);
        }
      }
    } catch {
      // ignored
    }
  }

  public async Task<AuthTokens> LoginAsync(string email, string password, UserRole role, CancellationToken ct) {
    var user = await userManager.FindByEmailAsync(email);

    if (user == null || user.IsDeleted || user.Role != role || !await userManager.CheckPasswordAsync(user, password)) {
      throw new WorkflowException("Thông tin đăng nhập không chính xác", 401);
    }

    if (await userManager.IsLockedOutAsync(user)) {
      throw new WorkflowException("Tài khoản đang bị khóa. Vui lòng liên hệ Admin.", 403);
    }

    var userModel = ToUserModel(user);
    return new AuthTokens(
      jwtService.GenerateAccessToken(userModel),
      jwtService.GenerateRefreshToken(userModel)
    );
  }

  private static User ToUserModel(AppUser u) {
    return new User {
      Id = u.Id,
      Email = u.Email!,
      FullName = u.FullName,
      PhoneNumber = u.PhoneNumber,
      Url = u.Url,
      IsActive = u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow,
      Role = u.Role,
      SecurityStamp = u.SecurityStamp
    };
  }
}
