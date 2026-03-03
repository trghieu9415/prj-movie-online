using MassTransit;
using MediatR;
using Mv.Application.Ports.Realtime;
using Mv.Application.UseCases.System.ExpireOrder;

namespace Mv.Worker.Consumers.Command;

public class ExpireOrderConsumer(
  IMediator mediator,
  IShowtimeNotifier showtimeNotifier
) : IConsumer<ExpireOrderCommand> {
  public async Task Consume(ConsumeContext<ExpireOrderCommand> context) {
    var command = context.Message;
    await mediator.Send(command);
  }
}
