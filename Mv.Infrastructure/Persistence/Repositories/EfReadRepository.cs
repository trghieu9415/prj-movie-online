using System.Collections;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs.Base;
using Mv.Application.Repositories;
using Mv.Domain.Base;

namespace Mv.Infrastructure.Persistence.Repositories;

public class EfReadRepository<TEntity, TDto>(
  AppDbContext dbContext,
  IMapper mapper
) : IReadRepository<TEntity, TDto>
  where TEntity : BaseEntity
  where TDto : IdDto {
  private static readonly string[] AllExpandableProperties = typeof(TDto).GetProperties()
    .Where(p =>
      (p.PropertyType.IsClass && p.PropertyType != typeof(string)) ||
      typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
    .Select(p => p.Name)
    .ToArray();

  protected readonly AppDbContext DbContext = dbContext;
  protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

  public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken ct = default) {
    return await DbSet
      .AsNoTracking()
      .Where(x => x.Id == id && !x.IsDeleted)
      .ProjectTo<TDto>(mapper.ConfigurationProvider, null, AllExpandableProperties)
      .FirstOrDefaultAsync(ct);
  }

  public async Task<(int total, List<TDto> entities)> GetAsync(
    Expression<Func<TEntity, bool>>? criteria = null,
    List<Expression<Func<TEntity, object>>>? includes = null,
    int? page = null,
    int? pageSize = null,
    CancellationToken ct = default
  ) {
    var query = DbSet.AsNoTracking().Where(x => !x.IsDeleted);

    if (criteria != null) {
      query = query.Where(criteria);
    }

    var total = await query.CountAsync(ct);

    if (includes != null && includes.Count != 0) {
      query = includes.Aggregate(query, (current, include) => current.Include(include));
    }

    query = query.OrderByDescending(x => x.CreatedAt);

    if (page.HasValue && pageSize.HasValue) {
      var validPageSize = Math.Max(1, pageSize.Value);
      var totalPages = (total + validPageSize - 1) / validPageSize;
      var validPage = Math.Max(1, totalPages);

      var skip = (validPage - 1) * validPageSize;
      query = query.Skip(skip).Take(validPageSize);
    }

    var entities = await query
      .ProjectTo<TDto>(mapper.ConfigurationProvider)
      .ToListAsync(ct);

    return (total, entities);
  }
}
