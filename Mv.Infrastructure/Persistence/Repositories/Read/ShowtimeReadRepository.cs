using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs;
using Mv.Application.Repositories.Read;
using Mv.Domain.Entities;
using Mv.Domain.Enums;
using Mv.Domain.ValueObjects;

namespace Mv.Infrastructure.Persistence.Repositories.Read;

public class ShowtimeReadRepository(
  AppDbContext dbContext,
  IMapper mapper
) : EfReadRepository<Showtime, ShowtimeDto>(dbContext, mapper), IShowtimeReadRepository {
  public async Task<List<Guid>> GetLockedSeatIdsAsync(Guid showtimeId, CancellationToken ct = default) {
    var lockedSeatIds = await DbContext.Set<Order>()
      .AsNoTracking()
      .Where(o => o.ShowtimeId == showtimeId)
      .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed)
      .SelectMany(o => o.Tickets).Select(t => t.SeatSnapshot.SeatId)
      .Distinct()
      .ToListAsync(ct);

    return lockedSeatIds;
  }

  public async Task<(MovieSnapshot movie, string auditoriumName)?> GetMovieAndAuditoriumAsync(
    Guid id,
    CancellationToken ct = default
  ) {
    var result = await DbContext.Set<Listing>()
      .AsNoTracking()
      .SelectMany(l => l.Showtimes, (l, s) => new { l, s })
      .Where(x => x.s.Id == id && !x.s.IsDeleted)
      .Select(x => new {
        Movie = DbContext.Set<Movie>()
          .Where(m => m.Id == x.l.MovieId)
          .Select(m => new MovieSnapshot {
            Id = m.Id,
            Name = m.Name,
            PosterUrl = m.PosterUrl
          })
          .FirstOrDefault(),
        AuditoriumName = DbContext.Set<Auditorium>()
          .Where(a => a.Id == x.s.AuditoriumId)
          .Select(a => a.Name)
          .FirstOrDefault()
      }).FirstOrDefaultAsync(ct);

    if (result?.Movie == null || result.AuditoriumName == null) {
      return null;
    }

    return (result.Movie, result.AuditoriumName);
  }
}
