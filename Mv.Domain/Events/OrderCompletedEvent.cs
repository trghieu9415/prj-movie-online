using Mv.Domain.Base.Event;

namespace Mv.Domain.Events;

public record OrderCompletedEvent(
  Guid OrderId,
  Guid CustomerId,
  string Email,
  string CustomerName,
  Guid ShowtimeId,
  ICollection<Guid> SeatIds
) : DomainEvent {
  public override Guid AggregateId => OrderId;
}
