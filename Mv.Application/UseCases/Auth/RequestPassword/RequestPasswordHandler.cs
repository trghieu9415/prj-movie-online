using MediatR;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Auth.RequestPassword;

public class RequestPasswordHandler(IAuthService authService) : IRequestHandler<RequestPasswordCommand, bool> {
  public async Task<bool> Handle(RequestPasswordCommand request, CancellationToken ct) {
    await authService.RequestPasswordAsync(request.Email, ct);
    return true;
  }
}
