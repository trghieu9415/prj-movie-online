using System.Security.Claims;
using Mv.Application.Models;

namespace Mv.Infrastructure.Services.Abstractions;

public interface IJwtService {
  TokenModel GenerateAccessToken(User user);
  TokenModel GenerateRefreshToken(User user);
  ClaimsPrincipal? ValidateToken(string token);
}
