using Domain.Events;
using MassTransit;
using Mv.Application.Constants;
using Mv.Application.Ports.Realtime;

namespace Mv.Worker.Consumers.Event;

public class OrderCanceledConsumer(
  IShowtimeNotifier showtimeNotifier
) : IConsumer<OrderCanceledEvent> {
  public async Task Consume(ConsumeContext<OrderCanceledEvent> context) {
    var msg = context.Message;
    await showtimeNotifier.SendToShowtimeGroup(
      msg.ShowtimeId,
      ClientMethods.SeatReleased,
      new {
        msg.SeatIds
      }
    );
  }
}
