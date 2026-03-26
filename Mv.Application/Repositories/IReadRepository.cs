using System.Linq.Expressions;
using Mv.Application.DTOs.Base;
using Mv.Domain.Base;

namespace Mv.Application.Repositories;

public interface IReadRepository<TEntity, TDto>
  where TEntity : BaseEntity
  where TDto : IdDto {
  Task<TDto?> GetByIdAsync(
    Guid id,
    CancellationToken ct
  );

  Task<(int total, List<TDto> entities)> GetAsync(
    Expression<Func<TEntity, bool>>? criteria = null,
    List<Expression<Func<TEntity, object>>>? includes = null,
    int? page = null,
    int? pageSize = null,
    CancellationToken ct = default
  );

  // Task<(int total, List<TDto> entities)> GetDeletedAsync(
  //   Expression<Func<TEntity, bool>>? criteria = null,
  //   string[]? expands = null,
  //   int? page = null,
  //   int? pageSize = null,
  //   CancellationToken ct = default
  // );
}
