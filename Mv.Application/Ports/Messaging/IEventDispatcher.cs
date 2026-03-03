namespace Mv.Application.Ports.Messaging;

public interface IEventDispatcher {
  Task DispatchEventsAsync(CancellationToken ct = default);
}
