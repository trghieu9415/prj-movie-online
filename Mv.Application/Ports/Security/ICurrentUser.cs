using Mv.Application.Models;

namespace Mv.Application.Ports.Security;

public interface ICurrentUser {
  Guid UserId { get; init; }
  string FullName { get; init; }
  string Email { get; init; }
  public string? SecurityStamp { get; init; }
  public UserRole Role { get; init; }
}
