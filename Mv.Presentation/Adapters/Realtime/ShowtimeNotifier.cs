using Mv.Application.Ports.Realtime;

namespace Mv.Presentation.Adapters.Realtime;

public class ShowtimeNotifier : IShowtimeNotifier {
  public Task SendToShowtimeGroup(Guid showtimeId, string method, object data, CancellationToken ct = default) {
    throw new NotImplementedException();
  }
}
