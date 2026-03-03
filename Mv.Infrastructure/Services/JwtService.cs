using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Mv.Application.Models;
using Mv.Infrastructure.Configs.Options;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Services;

public class JwtService(JwtOptions jwtOptions) : IJwtService {
  public TokenModel GenerateAccessToken(User user) {
    var claims = new List<Claim> {
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimTypes.Email, user.Email),
      new(ClaimTypes.Name, user.FullName),
      new(ClaimTypes.Role, nameof(user.Role)),
      new("security_stamp", user.SecurityStamp ?? ""),
      new("token_type", "access"),
      new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
    return GenerateToken(claims, jwtOptions.AccessExpiration);
  }

  public TokenModel GenerateRefreshToken(User user) {
    var claims = new List<Claim> {
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new("security_stamp", user.SecurityStamp ?? ""),
      new("token_type", "refresh"),
      new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
    return GenerateToken(claims, jwtOptions.RefreshExpiration);
  }

  public ClaimsPrincipal? ValidateToken(string token) {
    if (string.IsNullOrEmpty(token)) {
      return null;
    }

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(jwtOptions.Secret!);

    try {
      var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ClockSkew = TimeSpan.Zero
      }, out _);

      return principal;
    } catch {
      return null;
    }
  }

  // NOTE: ========== [Helpers] ==========
  private TokenModel GenerateToken(IEnumerable<Claim> claims, int expirationMinutes) {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expiry = DateTime.UtcNow.AddMinutes(expirationMinutes);

    var token = new JwtSecurityToken(
      jwtOptions.Issuer,
      jwtOptions.Audience,
      claims,
      expires: expiry,
      signingCredentials: creds
    );

    return new TokenModel {
      Token = new JwtSecurityTokenHandler().WriteToken(token),
      ExpiredAt = expiry
    };
  }
}
