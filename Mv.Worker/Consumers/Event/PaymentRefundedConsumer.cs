using MassTransit;
using MediatR;
using Mv.Application.UseCases.System.RefundOrder;
using Mv.Domain.Events;

namespace Mv.Worker.Consumers.Event;

public class PaymentRefundedConsumer(
  IMediator mediator
) : IConsumer<PaymentRefundedEvent> {
  public async Task Consume(ConsumeContext<PaymentRefundedEvent> context) {
    var msg = context.Message;
    var command = new RefundOrderCommand(msg.OrderId);
    await mediator.Send(command, context.CancellationToken);
  }
}
