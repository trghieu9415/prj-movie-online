using Domain.Entities;
using Domain.ValueObjects;
using Mv.Application.DTOs;

namespace Mv.Application.Repositories.Read;

public interface ISeatReadRepository : IReadRepository<Seat, SeatDto> {
  Task<List<SeatSnapshot>> GetValidSeatsForShowtimeAsync(
    List<Guid> seatIds,
    Guid showtimeId,
    CancellationToken ct = default
  );
}
