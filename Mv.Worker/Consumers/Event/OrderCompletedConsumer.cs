using MassTransit;
using Mv.Application.Constants;
using Mv.Application.Ports.Messaging;
using Mv.Application.Ports.Realtime;
using Mv.Domain.Events;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Worker.Consumers.Event;

public class OrderCompletedConsumer(
  IBackgroundTaskQueue taskQueue
) : IConsumer<OrderCompletedEvent> {
  public async Task Consume(ConsumeContext<OrderCompletedEvent> context) {
    var msg = context.Message;
    await taskQueue.QueueAsync<IUserNotifier>((uN, ctx) =>
      uN.SendToUser(
        msg.CustomerId,
        ClientMethods.OrderCompleted,
        new { msg.OrderId },
        ctx
      )
    );

    await taskQueue.QueueAsync<IEmailService>((es, ctx) =>
      es.SendOrderConfirmationEmailAsync(msg.Email, msg.CustomerName, msg.OrderId.ToString(), ctx)
    );
  }
}
