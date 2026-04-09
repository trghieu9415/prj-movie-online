namespace Mv.Application.Ports.State;

public interface ISeatStateStore {
  Task HoldSeatAsync(Guid showtimeId, Guid userId, Guid seatId, CancellationToken ct = default);
  Task ReleaseSeatAsync(Guid showtimeId, Guid userId, Guid seatId, CancellationToken ct = default);
  Task ReleaseAllSeatsAsync(Guid showtimeId, Guid userId, CancellationToken ct = default);
  Task<List<Guid>> GetAllHeldSeatsAsync(Guid showtimeId, CancellationToken ct = default);
  Task<List<Guid>> GetHeldSeatsByUserAsync(Guid showtimeId, Guid userId, CancellationToken ct = default);
}
