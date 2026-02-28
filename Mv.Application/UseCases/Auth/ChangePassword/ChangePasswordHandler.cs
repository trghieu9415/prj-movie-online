using MediatR;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Auth.ChangePassword;

public class ChangePasswordHandler(
  IAuthService authService,
  ICurrentUser currentUser
) : IRequestHandler<ChangePasswordCommand, bool> {
  public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken ct) {
    var user = await authService.ChangePasswordAsync(currentUser.Id, request.OldPassword, request.NewPassword, ct);
    return true;
  }
}
