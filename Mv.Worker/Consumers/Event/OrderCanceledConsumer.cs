using MassTransit;
using Mv.Application.Ports.Messaging;
using Mv.Application.Ports.Realtime;
using Mv.Domain.Events;

namespace Mv.Worker.Consumers.Event;

public class OrderCanceledConsumer(
  IBackgroundTaskQueue taskQueue
) : IConsumer<OrderCanceledEvent> {
  public async Task Consume(ConsumeContext<OrderCanceledEvent> context) {
    var msg = context.Message;

    await taskQueue.QueueAsync<IShowtimeNotifier>((sN, ctx) =>
      sN.NotifySeatReleasedAsync(msg.ShowtimeId, msg.SeatIds, ctx)
    );
  }
}
