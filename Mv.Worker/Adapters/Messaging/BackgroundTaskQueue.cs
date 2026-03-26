using System.Linq.Expressions;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Mv.Application.Ports.Messaging;

namespace Mv.Worker.Adapters.Messaging;

public class BackgroundTaskQueue : IBackgroundTaskQueue {
  private readonly Channel<Func<CancellationToken, IServiceProvider, ValueTask>> _queue;

  public BackgroundTaskQueue(int capacity) {
    var options = new BoundedChannelOptions(capacity) {
      FullMode = BoundedChannelFullMode.Wait
    };
    _queue = Channel.CreateBounded<Func<CancellationToken, IServiceProvider, ValueTask>>(options);
  }

  public async ValueTask QueueAsync<T>(Expression<Func<T, Task>> workItem) where T : notnull {
    var compiledMethod = workItem.Compile();
    await _queue.Writer.WriteAsync(Wrapper);
    return;

    async ValueTask Wrapper(CancellationToken ct, IServiceProvider sp) {
      var service = sp.GetRequiredService<T>();
      await compiledMethod(service);
    }
  }

  public async ValueTask<Func<CancellationToken, IServiceProvider, ValueTask>> DequeueAsync(CancellationToken ct) {
    return await _queue.Reader.ReadAsync(ct);
  }
}
