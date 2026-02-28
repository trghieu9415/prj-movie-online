using MediatR;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Auth.Login;

public class LoginHandler(
  IAuthService authService
) : IRequestHandler<LoginCommand, LoginResult> {
  public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct) {
    var authTokens = await authService.LoginAsync(request.Email, request.Password, request.Role, ct);
    return new LoginResult(authTokens);
  }
}
