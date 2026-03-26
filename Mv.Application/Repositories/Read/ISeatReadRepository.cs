using Mv.Application.DTOs;
using Mv.Domain.Entities;
using Mv.Domain.ValueObjects;

namespace Mv.Application.Repositories.Read;

public interface ISeatReadRepository : IReadRepository<Seat, SeatDto> {
  Task<List<SeatSnapshot>> GetValidSeatsForShowtimeAsync(
    List<Guid> seatIds,
    Guid showtimeId,
    CancellationToken ct = default
  );
}
