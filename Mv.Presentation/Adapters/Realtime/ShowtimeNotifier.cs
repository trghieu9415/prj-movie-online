using Microsoft.AspNetCore.SignalR;
using Mv.Application.Ports.Realtime;
using Mv.Presentation.Hubs;

namespace Mv.Presentation.Adapters.Realtime;

public class ShowtimeNotifier(IHubContext<ShowtimeHub> hubContext) : IShowtimeNotifier {
  public async Task
    NotifySeatReleasedAsync(Guid showtimeId, IEnumerable<Guid> seatIds, CancellationToken ct = default) {
    await hubContext.Clients.Group(showtimeId.ToString()).SendAsync("SeatReleased", seatIds, ct);
  }

  public async Task NotifySeatHeldAsync(Guid showtimeId, Guid seatId, CancellationToken ct = default) {
    await hubContext.Clients.Group(showtimeId.ToString()).SendAsync("SeatHeld", seatId, ct);
  }

  public async Task NotifySeatSoldAsync(Guid showtimeId, IEnumerable<Guid> seatIds, CancellationToken ct = default) {
    await hubContext.Clients.Group(showtimeId.ToString()).SendAsync("SeatSold", seatIds, ct);
  }
}
