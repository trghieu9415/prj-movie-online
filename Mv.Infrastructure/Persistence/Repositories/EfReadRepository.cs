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
  protected readonly AppDbContext DbContext = dbContext;
  protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();
  protected readonly IMapper Mapper = mapper;

  public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken ct = default) {
    return await DbSet
      .AsNoTracking()
      .Where(x => x.Id == id && !x.IsDeleted)
      .ProjectTo<TDto>(Mapper.ConfigurationProvider)
      .FirstOrDefaultAsync(ct);
  }

  public virtual async Task<(int total, List<TDto> entities)> GetAsync(
    Expression<Func<TEntity, bool>>? criteria = null,
    List<Expression<Func<TEntity, object>>>? includes = null,
    CancellationToken ct = default
  ) {
    var query = DbSet.AsNoTracking().Where(x => !x.IsDeleted);

    if (criteria != null) {
      query = query.Where(criteria);
    }

    if (includes != null) {
      query = includes.Aggregate(query, (current, include) => current.Include(include));
    }

    var total = await query.CountAsync(ct);
    var entities = await query.ProjectTo<TDto>(Mapper.ConfigurationProvider).ToListAsync(ct);

    return (total, entities);
  }

  public virtual async Task<(int total, List<TDto> entities)> GetDeletedAsync(
    Expression<Func<TEntity, bool>>? criteria = null,
    List<Expression<Func<TEntity, object>>>? includes = null,
    CancellationToken ct = default
  ) {
    var query = DbSet.AsNoTracking().Where(x => x.IsDeleted);

    if (criteria != null) {
      query = query.Where(criteria);
    }

    if (includes != null) {
      query = includes.Aggregate(query, (current, include) => current.Include(include));
    }

    var total = await query.CountAsync(ct);
    var entities = await query.ProjectTo<TDto>(Mapper.ConfigurationProvider).ToListAsync(ct);

    return (total, entities);
  }
}
