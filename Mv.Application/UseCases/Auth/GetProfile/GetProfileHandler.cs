using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Auth.GetProfile;

public class GetProfileHandler(
  ICurrentUser currentUser,
  IUserService userService
) : IRequestHandler<GetProfileQuery, GetProfileResult> {
  public async Task<GetProfileResult> Handle(GetProfileQuery request, CancellationToken ct) {
    var user =
      await userService.GetByIdAsync(currentUser.Id, currentUser.Role, ct)
      ?? throw new WorkflowException("Không tìm thấy thông tin người dùng", 404);

    return new GetProfileResult(user);
  }
}
