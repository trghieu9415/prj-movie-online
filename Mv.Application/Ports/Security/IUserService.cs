using Mv.Application.Models;

namespace Mv.Application.Ports.Security;

public interface IUserService {
  Task<User?> GetByIdAsync(Guid id, UserRole? role, CancellationToken ct = default);
  Task<(int total, List<User> users)> GetAsync(UserRole? role, CancellationToken ct = default);
  Task<(int total, List<User> users)> GetDeletedAsync(UserRole? role, CancellationToken ct = default);

  Task<Guid> CreateAsync(
    User user, string password,
    UserRole role, CancellationToken ct = default
  );

  Task UpdateAsync(User user, CancellationToken ct = default);
  Task DeleteAsync(Guid id, CancellationToken ct = default);
  Task RestoreAsync(Guid id, CancellationToken ct = default);

  Task LockAsync(Guid id, CancellationToken ct = default);
  Task UnlockAsync(Guid id, CancellationToken ct = default);
}
