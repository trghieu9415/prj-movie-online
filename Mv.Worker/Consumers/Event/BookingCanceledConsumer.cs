using Domain.Events;
using MassTransit;

namespace Mv.Worker.Consumers.Event;

public class BookingCanceledConsumer : IConsumer<BookingCanceledEvent> {
  public Task Consume(ConsumeContext<BookingCanceledEvent> context) {
    throw new NotImplementedException();
  }
}
