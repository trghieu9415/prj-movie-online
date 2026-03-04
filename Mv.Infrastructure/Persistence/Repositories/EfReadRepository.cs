using System.Collections;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Base;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs.Base;
using Mv.Application.Repositories;

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

  public virtual async Task<(int total, List<TDto> entities)> GetAsync(
    Expression<Func<TEntity, bool>>? criteria = null,
    string[]? expands = null,
    int? page = null,
    int? pageSize = null,
    CancellationToken ct = default
  ) {
    var query = DbSet.AsNoTracking().Where(x => !x.IsDeleted);

    if (criteria != null) {
      query = query.Where(criteria);
    }

    var total = await query.CountAsync(ct);

    if (page.HasValue && pageSize.HasValue) {
      var validPage = Math.Max(1, page.Value);
      var validPageSize = Math.Max(1, pageSize.Value);

      var skip = (validPage - 1) * validPageSize;
      query = query.Skip(skip).Take(validPageSize);
    }

    var entities = await query
      .ProjectTo<TDto>(mapper.ConfigurationProvider, null, expands ?? [])
      .ToListAsync(ct);

    return (total, entities);
  }

  public virtual async Task<(int total, List<TDto> entities)> GetDeletedAsync(
    Expression<Func<TEntity, bool>>? criteria = null,
    string[]? expands = null,
    int? page = null,
    int? pageSize = null,
    CancellationToken ct = default
  ) {
    var query = DbSet.AsNoTracking().Where(x => x.IsDeleted);

    if (criteria != null) {
      query = query.Where(criteria);
    }

    var total = await query.CountAsync(ct);

    if (page.HasValue && pageSize.HasValue) {
      var validPage = Math.Max(1, page.Value);
      var validPageSize = Math.Max(1, pageSize.Value);

      var skip = (validPage - 1) * validPageSize;
      query = query.Skip(skip).Take(validPageSize);
    }

    var entities = await query
      .ProjectTo<TDto>(mapper.ConfigurationProvider, null, expands ?? [])
      .ToListAsync(ct);

    return (total, entities);
  }
}
