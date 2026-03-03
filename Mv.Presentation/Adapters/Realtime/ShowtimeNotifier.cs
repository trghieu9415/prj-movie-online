using Microsoft.AspNetCore.SignalR;
using Mv.Application.Ports.Realtime;
using Mv.Presentation.Hubs;

namespace Mv.Presentation.Adapters.Realtime;

public class ShowtimeNotifier(IHubContext<ShowtimeHub> hubContext) : IShowtimeNotifier {
  public async Task SendToShowtimeGroup(
    Guid showtimeId,
    string method,
    object data,
    CancellationToken ct = default
  ) {
    await hubContext.Clients
      .Group(showtimeId.ToString())
      .SendAsync(method, data, ct);
  }
}
