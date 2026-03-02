using Domain.Base.Event;

namespace Domain.Events;

public record PlanCanceledEvent(
  Guid PlanId
) : DomainEvent {
  public override Guid AggregateId => PlanId;
}
