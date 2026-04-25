using MassTransit;
using MediatR;
using Mv.Application.Constants;
using Mv.Application.Ports.Messaging;
using Mv.Application.Ports.Realtime;
using Mv.Application.UseCases.System.MarkOrderAsPaid;
using Mv.Domain.Events;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Worker.Consumers.Event;

public class PaymentCompletedConsumer(
  IMediator mediator,
  IBackgroundTaskQueue taskQueue
) : IConsumer<PaymentCompletedEvent> {
  public async Task Consume(ConsumeContext<PaymentCompletedEvent> context) {
    var msg = context.Message;

    Console.WriteLine($"[+] Payment Completed For Order Id: {msg.OrderId}");

    await mediator.Send(new MarkOrderAsPaidCommand(msg.OrderId), context.CancellationToken);

    await taskQueue.QueueAsync<IEmailService>((es, ctx) =>
      es.SendOrderConfirmationEmailAsync(msg.CustomerEmail, msg.CustomerId.ToString(), msg.OrderId.ToString(), ctx)
    );

    await GeneratePdfSimulation(msg.OrderId.ToString());
    Console.WriteLine($"[+] Pdf Ticket Generated: {msg.OrderId}");

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

  private static async Task GeneratePdfSimulation(string orderId) {
    await Task.Delay(3000);
    Console.WriteLine($"[+] Generating Ticket For Order Id: {orderId} - take 3s");
  }
}
