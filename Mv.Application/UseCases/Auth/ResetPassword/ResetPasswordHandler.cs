using MediatR;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Auth.ResetPassword;

public class ResetPasswordHandler(IAuthService authService) : IRequestHandler<ResetPasswordCommand, bool> {
  public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken ct) {
    await authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword, ct);
    return true;
  }
}
