using Mv.Application.Models;

namespace Mv.Application.Ports.Security;

public interface ICurrentUser {
  Guid Id { get; init; }
  string FullName { get; init; }
  public UserRole Role { get; init; }
}
