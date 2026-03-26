using Microsoft.AspNetCore.SignalR;
using Mv.Application.Ports.Realtime;
using Mv.Presentation.Hubs;

namespace Mv.Presentation.Adapters.Realtime;

public class UserNotifier(IHubContext<UserHub> hubContext) : IUserNotifier {
  public async Task SendToUser(Guid userId, string method, object data, CancellationToken ct = default) {
    await hubContext.Clients.User(userId.ToString()).SendAsync(method, data, ct);
  }
}
