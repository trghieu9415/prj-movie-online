using MassTransit;
using Mv.Application.Constants;
using Mv.Application.Ports.Realtime;
using Mv.Application.UseCases.System.ExpireOrder;
using Mv.Domain.Events;

namespace Mv.Worker.Consumers.Event;

public class OrderPlacedConsumer(
  IShowtimeNotifier showtimeNotifier,
  IMessageScheduler messageScheduler
) : IConsumer<OrderPlacedEvent> {
  public async Task Consume(ConsumeContext<OrderPlacedEvent> context) {
    var msg = context.Message;
    await messageScheduler.SchedulePublish(
      TimeSpan.FromMinutes(15),
      new ExpireOrderCommand(msg.OrderId)
    );

    await showtimeNotifier.SendToShowtimeGroup(
      msg.ShowtimeId,
      ClientMethods.SeatLocked,
      new {
        msg.SeatIds
      }
    );
  }
}
