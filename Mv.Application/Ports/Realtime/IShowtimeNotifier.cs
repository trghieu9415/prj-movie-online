namespace Mv.Application.Ports.Realtime;

public interface IShowtimeNotifier {
  Task SendToShowtimeGroup(Guid showtimeId, string method, object data, CancellationToken ct = default);
}
