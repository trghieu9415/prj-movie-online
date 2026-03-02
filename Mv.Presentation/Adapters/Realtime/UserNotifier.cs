using Mv.Application.Ports.Realtime;

namespace Mv.Presentation.Adapters.Realtime;

public class UserNotifier : IUserNotifier {
  public Task SendToUser(Guid userId, string method, object data, CancellationToken ct = default) {
    throw new NotImplementedException();
  }
}
