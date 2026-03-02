using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs;
using Mv.Application.Ports.Repositories.Read;
using Mv.Infrastructure.Persistence;

namespace Mv.Infrastructure.Adapters.Repositories.Read;

public class PlanReadRepository(AppDbContext dbContext, IMapper mapper)
  : EfReadRepository<Plan, PlanDto>(dbContext, mapper), IPlanReadRepository {
  public async Task<PlanDto?> GetPlanOverviewAsync(Guid id, CancellationToken ct = default) {
    var movies = DbContext.Set<Movie>().AsNoTracking();

    var planDto = await DbSet.AsNoTracking()
      .Where(x => x.Id == id && !x.IsDeleted)
      .Select(p => new PlanDto(
        p.Id, p.Name, p.Year, p.Month, p.Week,
        p.Listings.Where(l => !l.IsDeleted).Select(l => new ListingDto(
          l.Id, movies.Where(m => m.Id == l.MovieId && !m.IsDeleted)
            .Select(m => new MovieDto(m.Id, m.Name, m.Duration, m.PosterUrl))
            .FirstOrDefault(),
          new List<ShowtimeDto>()
        )).ToList()
      ))
      .FirstOrDefaultAsync(ct);

    return planDto;
  }
}
