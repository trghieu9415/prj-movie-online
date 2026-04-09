using MassTransit;
using Mv.Application.Ports.Messaging;
using Mv.Application.Ports.Realtime;
using Mv.Application.UseCases.System.ExpireOrder;
using Mv.Domain.Events;

namespace Mv.Worker.Consumers.Event;

public class OrderPlacedConsumer(
  IShowtimeNotifier showtimeNotifier,
  IMessageScheduler messageScheduler,
  IBackgroundTaskQueue taskQueue
) : IConsumer<OrderPlacedEvent> {
  public async Task Consume(ConsumeContext<OrderPlacedEvent> context) {
    var msg = context.Message;
    await taskQueue.QueueAsync<IMessageScheduler>((mS, ctx) =>
      mS.SchedulePublish(
        TimeSpan.FromMinutes(15),
        new ExpireOrderCommand(msg.OrderId),
        ctx
      )
    );

    await taskQueue.QueueAsync<IShowtimeNotifier>((sN, ctx) =>
      sN.NotifySeatSoldAsync(msg.ShowtimeId, msg.SeatIds, ctx)
    );
  }
}
