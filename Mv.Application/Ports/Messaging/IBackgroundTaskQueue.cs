using System.Linq.Expressions;

namespace Mv.Application.Ports.Messaging;

public interface IBackgroundTaskQueue {
  public ValueTask QueueAsync<T>(Expression<Func<T, CancellationToken, Task>> workItem) where T : notnull;
  public ValueTask QueueAsync(Func<CancellationToken, Task> workItem);
  ValueTask<Func<CancellationToken, IServiceProvider, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}
