using Microsoft.AspNetCore.SignalR;

namespace Mv.Presentation.Hubs;

public class ShowtimeHub : Hub {
  public async Task JoinShowtime(Guid showtimeId) {
    await Groups.AddToGroupAsync(Context.ConnectionId, showtimeId.ToString());
  }

  public async Task LeaveShowtime(Guid showtimeId) {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, showtimeId.ToString());
  }
}
