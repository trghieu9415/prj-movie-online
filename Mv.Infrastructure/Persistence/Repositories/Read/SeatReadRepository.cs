using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs;
using Mv.Application.Repositories.Read;

namespace Mv.Infrastructure.Persistence.Repositories.Read;

public class SeatReadRepository(
  AppDbContext dbContext,
  IMapper mapper
) : EfReadRepository<Seat, SeatDto>(dbContext, mapper), ISeatReadRepository {
  public async Task<List<SeatSnapshot>> GetValidSeatsForShowtimeAsync(
    List<Guid> seatIds,
    Guid showtimeId,
    CancellationToken ct = default
  ) {
    if (seatIds.Count == 0) {
      return [];
    }

    var validSeats = await DbContext.Set<Seat>()
      .AsNoTracking()
      .Where(s => seatIds.Contains(s.Id) && !s.IsDeleted)
      .Where(s => DbContext.Set<Showtime>()
        .Any(st => st.Id == showtimeId && st.AuditoriumId == s.AuditoriumId))
      .Select(s => new SeatSnapshot(s.Id, s.Row, s.Number))
      .ToListAsync(ct);
    return validSeats;
  }
}
