using MediatR;
using Mv.Application.Models;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Auth.Register;

public class RegisterHandler(IAuthService authService) : IRequestHandler<RegisterCommand, RegisterResult> {
  public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken ct) {
    var user = new User {
      FullName = request.FullName,
      Email = request.Email,
      Role = UserRole.Customer
    };

    var tokens = await authService.RegisterAsync(user, request.Password, ct);
    return new RegisterResult(tokens);
  }
}
