using Domain.Entities;
using Domain.ValueObjects;
using Mv.Application.DTOs;

namespace Mv.Application.Repositories.Read;

public interface IShowtimeReadRepository : IReadRepository<Showtime, ShowtimeDto> {
  Task<List<Guid>> GetLockedSeatIdsAsync(Guid showtimeId, CancellationToken ct = default);

  Task<(MovieSnapshot movie, string auditoriumName)?> GetMovieAndAuditoriumAsync(
    Guid id,
    CancellationToken ct = default
  );
}
