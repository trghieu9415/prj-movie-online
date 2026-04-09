using System.Security.Claims;
using Mv.Application.Exceptions;
using Mv.Application.Models;
using Mv.Application.Ports.Security;

namespace Mv.Presentation.Adapters.Security;

public class CurrentUser : ICurrentUser {
  public CurrentUser(IHttpContextAccessor accessor) {
    var user = accessor.HttpContext?.User;
    var id = user?.FindFirstValue(ClaimTypes.NameIdentifier);

    if (id != null) {
      Id = Guid.Parse(id);
      FullName = user?.Identity?.Name ?? "Guest";
      Email = user?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
      Role = user?.FindFirstValue(ClaimTypes.Role) == nameof(UserRole.Admin) ? UserRole.Admin : UserRole.Customer;
    } else {
      throw new WorkflowException("Token không hợp lệ", 401);
    }
  }

  public Guid Id { get; init; }
  public string FullName { get; init; }
  public string Email { get; init; }
  public UserRole Role { get; init; }
}
