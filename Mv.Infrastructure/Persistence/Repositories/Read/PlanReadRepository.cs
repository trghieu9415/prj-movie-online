using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs;
using Mv.Application.Repositories.Read;

namespace Mv.Infrastructure.Persistence.Repositories.Read;

public class PlanReadRepository(
  AppDbContext dbContext,
  IMapper mapper
) : EfReadRepository<Plan, PlanDto>(dbContext, mapper), IPlanReadRepository {
  public async Task<PlanDto?> GetPlanOverviewAsync(Guid id, CancellationToken ct = default) {
    var movies = DbContext.Set<Movie>().AsNoTracking();

    var planDto = await DbSet.AsNoTracking().AsSplitQuery()
      .Where(x => x.Id == id && !x.IsDeleted)
      .Select(p => new PlanDto(
        p.Id, p.Name, p.StartDate, p.EndDate,
        p.Listings
          .Where(l => !l.IsDeleted)
          .Select(l => new ListingDto(
            l.Id,
            movies
              .Where(m => m.Id == l.MovieId && !m.IsDeleted)
              .Select(m => new MovieDto(m.Id, m.Name, m.Duration, m.PosterUrl))
              .FirstOrDefault(),
            l.Showtimes.Select(s => new ShowtimeDto(
              s.Id, s.AuditoriumId, s.Date, s.StartAt, s.EndAt
            )).ToList()
          )).ToList()
      ))
      .FirstOrDefaultAsync(ct);

    return planDto;
  }

  public async Task<PlanDto?> GetCurrentPlanAsync(CancellationToken ct = default) {
    var today = DateOnly.FromDateTime(DateTime.UtcNow);

    var movies = DbContext.Set<Movie>().AsNoTracking();

    var currentPlanDto = await DbSet.AsNoTracking().AsSplitQuery()
      .Where(x => today >= x.StartDate && today <= x.EndDate && !x.IsDeleted)
      .Select(p => new PlanDto(
        p.Id, p.Name, p.StartDate, p.EndDate,
        p.Listings.Where(l => !l.IsDeleted).Select(l => new ListingDto(
          l.Id,
          movies.Where(m => m.Id == l.MovieId && !m.IsDeleted)
            .Select(m => new MovieDto(m.Id, m.Name, m.Duration, m.PosterUrl))
            .FirstOrDefault(),
          l.Showtimes.Select(s => new ShowtimeDto(
            s.Id, s.AuditoriumId, s.Date, s.StartAt, s.EndAt
          )).ToList()
        )).ToList()
      ))
      .FirstOrDefaultAsync(ct);
    return currentPlanDto;
  }
}
