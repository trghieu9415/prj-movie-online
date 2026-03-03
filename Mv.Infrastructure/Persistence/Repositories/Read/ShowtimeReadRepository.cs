using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs;
using Mv.Application.Repositories.Read;

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
}
