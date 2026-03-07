using MassTransit;
using MediatR;
using Mv.Application.UseCases.System.ExpireOrder;

namespace Mv.Worker.Consumers.Command;

public class ExpireOrderConsumer(
  IMediator mediator
) : IConsumer<ExpireOrderCommand> {
  public async Task Consume(ConsumeContext<ExpireOrderCommand> context) {
    var command = context.Message;
    await mediator.Send(command, context.CancellationToken);
  }
}
