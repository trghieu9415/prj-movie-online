using MassTransit;
using MediatR;
using Mv.Application.Constants;
using Mv.Application.Ports.Messaging;
using Mv.Application.Ports.Realtime;
using Mv.Application.UseCases.System.MarkOrderAsPaid;
using Mv.Domain.Events;

namespace Mv.Worker.Consumers.Event;

public class PaymentCompletedConsumer(
  IBackgroundTaskQueue taskQueue
) : IConsumer<PaymentCompletedEvent> {
  public async Task Consume(ConsumeContext<PaymentCompletedEvent> context) {
    var msg = context.Message;

    await taskQueue.QueueAsync<IMediator>((m, ctx) =>
      m.Send(new MarkOrderAsPaidCommand(msg.OrderId), ctx)
    );

    await taskQueue.QueueAsync<IUserNotifier>((uN, ctx) =>
      uN.SendToUser(
        msg.CustomerId,
        ClientMethods.PaymentSuccess,
        new {
          orderId = msg.OrderId,
          amount = msg.Amount
        },
        ctx
      )
    );
  }
}
