using Domain.Events;
using MassTransit;
using MediatR;
using Mv.Application.Constants;
using Mv.Application.Ports.Realtime;
using Mv.Application.UseCases.System.MarkOrderAsPaid;

namespace Mv.Worker.Consumers.Event;

public class PaymentCompletedConsumer(
  IMediator mediator,
  IUserNotifier userNotifier
) : IConsumer<PaymentCompletedEvent> {
  public async Task Consume(ConsumeContext<PaymentCompletedEvent> context) {
    var msg = context.Message;
    var command = new MarkOrderAsPaidCommand(msg.OrderId);
    await mediator.Send(command);

    await userNotifier.SendToUser(
      msg.CustomerId,
      ClientMethods.PaymentSuccess,
      new {
        orderId = msg.OrderId,
        amount = msg.Amount
      }
    );
  }
}
