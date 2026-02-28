using MediatR;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Auth.Refresh;

public class RefreshHandler(IAuthService authService) : IRequestHandler<RefreshCommand, RefreshResult> {
  public async Task<RefreshResult> Handle(RefreshCommand request, CancellationToken ct) {
    var tokens = await authService.RefreshAsync(request.RefreshToken, ct);
    return new RefreshResult(tokens);
  }
}
