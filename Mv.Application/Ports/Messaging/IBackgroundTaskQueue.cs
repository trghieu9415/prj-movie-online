using System.Linq.Expressions;

namespace Mv.Application.Ports.Messaging;

public interface IBackgroundTaskQueue {
  ValueTask QueueAsync<T>(Expression<Func<T, Task>> workItem) where T : notnull;
  ValueTask<Func<CancellationToken, IServiceProvider, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}
