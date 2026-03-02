using Domain.Base.Event;

namespace Domain.Events;

public record OrderCompletedEvent(
  Guid OrderId,
  Guid CustomerId,
  Guid ShowtimeId,
  List<Guid> SeatIds
) : DomainEvent {
  public override Guid AggregateId => OrderId;
}
