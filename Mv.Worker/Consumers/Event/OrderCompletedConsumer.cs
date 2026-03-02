using Domain.Events;
using MassTransit;

namespace Mv.Worker.Consumers.Event;

public class OrderCompletedConsumer : IConsumer<OrderCompletedEvent> {
  public Task Consume(ConsumeContext<OrderCompletedEvent> context) {
    throw new NotImplementedException();
  }
}
