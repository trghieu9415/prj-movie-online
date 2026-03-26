using MassTransit;
using Mv.Application.Constants;
using Mv.Application.Ports.Realtime;
using Mv.Domain.Events;

namespace Mv.Worker.Consumers.Event;

public class OrderCompletedConsumer(
  IUserNotifier userNotifier
) : IConsumer<OrderCompletedEvent> {
  public async Task Consume(ConsumeContext<OrderCompletedEvent> context) {
    var msg = context.Message;
    await userNotifier.SendToUser(
      msg.CustomerId,
      ClientMethods.OrderCompleted,
      new {
        msg.OrderId
      }
    );
  }
}
