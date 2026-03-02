using Domain.Base.Event;

namespace Domain.Events;

public record PlanPublishedEvent(
  Guid PlanId,
  ICollection<Guid> ListingIds,
  ICollection<Guid> MovieIds
) : DomainEvent {
  public override Guid AggregateId => PlanId;
}
