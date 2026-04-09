using MassTransit;
using MediatR;
using Mv.Application.UseCases.System.ExpireOrder;

namespace Mv.Worker.Consumers;

public class CommandConsumer(
  IMediator mediator
) : IConsumer<ExpireOrderCommand> {
  public async Task Consume(ConsumeContext<ExpireOrderCommand> context) {
    var command = context.Message;
    await mediator.Send(command, context.CancellationToken);
  }
}
