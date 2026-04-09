using MassTransit;
using MediatR;
using Mv.Application.Ports.Messaging;
using Mv.Application.UseCases.System.RefundOrder;
using Mv.Domain.Events;

namespace Mv.Worker.Consumers.Event;

public class PaymentRefundedConsumer(
  IBackgroundTaskQueue taskQueue
) : IConsumer<PaymentRefundedEvent> {
  public async Task Consume(ConsumeContext<PaymentRefundedEvent> context) {
    var msg = context.Message;

    await taskQueue.QueueAsync<IMediator>((m, ctx) =>
      m.Send(new RefundOrderCommand(msg.OrderId), ctx)
    );
  }
}
