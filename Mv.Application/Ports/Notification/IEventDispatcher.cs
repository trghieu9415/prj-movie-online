namespace Mv.Application.Ports.Notification;

public interface IEventDispatcher {
  Task DispatchEventsAsync(CancellationToken ct = default);
}
