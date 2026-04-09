namespace Mv.Application.Ports.Realtime;

public interface IShowtimeNotifier {


  Task NotifySeatReleasedAsync(Guid showtimeId, IEnumerable<Guid> seatIds, CancellationToken ct = default);
  Task NotifySeatHeldAsync(Guid showtimeId, Guid seatId, CancellationToken ct = default);
  Task NotifySeatSoldAsync(Guid showtimeId, IEnumerable<Guid> seatIds, CancellationToken ct = default);
}
