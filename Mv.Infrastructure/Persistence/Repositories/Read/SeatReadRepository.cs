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
    Guid auditoriumId,
    CancellationToken ct = default
  ) {
    var validSeats = await DbContext.Set<Seat>()
      .AsNoTracking()
      .Where(s => seatIds.Contains(s.Id))
      .Where(s => s.AuditoriumId == auditoriumId)
      .Where(s => !s.IsDeleted)
      .Select(s => new SeatSnapshot(s.Id, s.Row, s.Number))
      .ToListAsync(ct);
    return validSeats;
  }
}
