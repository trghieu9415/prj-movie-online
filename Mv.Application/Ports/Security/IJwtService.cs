using Mv.Application.Models;

namespace Mv.Application.Ports.Security;

public interface IJwtService {
  TokenModel GenerateAccessToken(User user);
  TokenModel GenerateRefreshToken(User user);
}
