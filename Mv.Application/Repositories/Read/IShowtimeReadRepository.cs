using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Application.Repositories.Read;

public interface IShowtimeReadRepository : IReadRepository<Showtime, ShowtimeDto> {
  Task<List<Guid>> GetLockedSeatIdsAsync(Guid showtimeId, CancellationToken ct = default);
}
