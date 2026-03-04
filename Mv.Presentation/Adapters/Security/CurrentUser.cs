using System.Security.Claims;
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
      Role = user?.FindFirstValue(ClaimTypes.Role) == nameof(UserRole.Admin) ? UserRole.Admin : UserRole.Customer;
    } else {
      Id = Guid.Empty;
    }
  }

  public Guid Id { get; init; }
  public string FullName { get; init; } = "Guest";
  public UserRole Role { get; init; }
}
