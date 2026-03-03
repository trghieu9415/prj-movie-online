using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Mv.Application.Exceptions;
using Mv.Application.Models;
using Mv.Application.Ports.Security;
using Mv.Infrastructure.Persistence.Identity;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Adapters.Security;

public class AuthService(
  UserManager<AppUser> userManager,
  IJwtService jwtService,
  IEmailService emailService,
  ICacheService cache
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

    await cache.SyncSecurityStampAsync(appUser.Id, appUser.SecurityStamp!, ct);

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

    var stampResult = await userManager.UpdateSecurityStampAsync(user);
    if (stampResult.Succeeded) {
      await cache.SyncSecurityStampAsync(user.Id, user.SecurityStamp!, ct);
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

      var stampResult = await userManager.UpdateSecurityStampAsync(user);
      if (stampResult.Succeeded) {
        await cache.SyncSecurityStampAsync(user.Id, user.SecurityStamp!, ct);
      }
    } catch {
      throw new WorkflowException("Mã xác nhận đã hết hạn hoặc không hợp lệ", 401);
    }
  }

  public async Task<bool> ValidateSecurityStampAsync(Guid userId, string tokenSecurityStamp, CancellationToken ct) {
    var cachedSecurityStamp = await cache.GetSecurityStampAsync(userId, ct);
    if (cachedSecurityStamp != null) {
      return tokenSecurityStamp == cachedSecurityStamp;
    }

    var user = await userManager.FindByIdAsync(userId.ToString());
    if (user == null || user.IsDeleted) {
      return false;
    }

    await cache.SyncSecurityStampAsync(userId, user.SecurityStamp!, ct);
    return tokenSecurityStamp == user.SecurityStamp;
  }

  public async Task<AuthTokens> RefreshAsync(string refreshToken, CancellationToken ct) {
    var principal = jwtService.ValidateToken(refreshToken);
    if (principal == null) {
      throw new WorkflowException("Token không hợp lệ hoặc đã hết hạn", 401);
    }

    var tokenType = principal.FindFirst("token_type")?.Value;
    if (tokenType != "refresh") {
      throw new WorkflowException("Token không đúng loại", 401);
    }

    var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var tokenStamp = principal.FindFirst("security_stamp")?.Value;

    if (await cache.IsBlacklistedAsync(jti!, ct)) {
      throw new WorkflowException("Phiên đăng nhập đã bị vô hiệu hóa", 401);
    }

    var user = await userManager.FindByIdAsync(userId!);
    if (user == null || user.IsDeleted) {
      throw new WorkflowException("Tài khoản không hợp lệ", 401);
    }

    if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow) {
      throw new WorkflowException("Tài khoản đã bị khóa", 403);
    }

    if (tokenStamp != user.SecurityStamp) {
      throw new WorkflowException("Thông tin bảo mật đã thay đổi, vui lòng đăng nhập lại", 401);
    }

    var expClaim = principal.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
    if (long.TryParse(expClaim, out var expUnix)) {
      var expiresAt = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
      var remainingTime = expiresAt - DateTime.UtcNow;
      if (remainingTime.TotalSeconds > 0) {
        await cache.BlacklistAsync(jti!, remainingTime, ct);
      }
    }

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
          var stampResult = await userManager.UpdateSecurityStampAsync(user);
          if (stampResult.Succeeded) {
            await cache.SyncSecurityStampAsync(user.Id, user.SecurityStamp!, ct);
          }
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

    await cache.SyncSecurityStampAsync(user.Id, user.SecurityStamp!, ct);

    var userModel = ToUserModel(user);
    return new AuthTokens(
      jwtService.GenerateAccessToken(userModel),
      jwtService.GenerateRefreshToken(userModel)
    );
  }

  // NOTE: ========== [Helper] ==========
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
