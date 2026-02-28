namespace Mv.Application.Ports.Realtime;

public interface IUserNotifier {
  Task SendToUser(Guid userId, string method, object data, CancellationToken ct = default);
}
