using Domain.Events;
using MassTransit;

namespace Mv.Worker.Consumers.Event;

public class SeatsReservedConsumer : IConsumer<SeatsReservedEvent> {
  public Task Consume(ConsumeContext<SeatsReservedEvent> context) {
    throw new NotImplementedException();
  }
}
