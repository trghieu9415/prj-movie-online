using Domain.Base.Event;

namespace Domain.Events;

public record OrderCanceledEvent(
  Guid OrderId,
  Guid CustomerId,
  Guid ShowtimeId,
  ICollection<Guid> SeatIds
) : DomainEvent {
  public override Guid AggregateId => OrderId;
}
