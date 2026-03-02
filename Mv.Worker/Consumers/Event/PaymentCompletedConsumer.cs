using Domain.Events;
using MassTransit;

namespace Mv.Worker.Consumers.Event;

public class PaymentCompletedConsumer : IConsumer<PaymentCompletedEvent> {
  public Task Consume(ConsumeContext<PaymentCompletedEvent> context) {
    throw new NotImplementedException();
  }
}
