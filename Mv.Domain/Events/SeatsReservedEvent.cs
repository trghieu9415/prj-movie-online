using Domain.Base.Event;

namespace Domain.Events;

public record SeatsReservedEvent(
  Guid OrderId,
  Guid ShowtimeId,
  ICollection<Guid> SeatIds
) : DomainEvent {
  public override Guid AggregateId => OrderId;
}
