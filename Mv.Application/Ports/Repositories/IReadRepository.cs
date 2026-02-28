using System.Linq.Expressions;
using Domain.Base;

namespace Mv.Application.Ports.Repositories;

public interface IReadRepository<T> where T : BaseEntity {
  Task<T?> GetByIdAsync(
    Guid id,
    CancellationToken ct = default
  );

  Task<(int total, List<T> entities)> GetAsync(
    Expression<Func<T, bool>>? criteria = null,
    List<Expression<Func<T, object>>>? includes = null,
    CancellationToken ct = default
  );

  Task<(int total, List<T> entities)> GetDeletedAsync(
    Expression<Func<T, bool>>? criteria = null,
    List<Expression<Func<T, object>>>? includes = null,
    CancellationToken ct = default
  );
}
