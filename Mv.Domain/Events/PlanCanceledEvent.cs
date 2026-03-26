using Mv.Domain.Base.Event;

namespace Mv.Domain.Events;

public record PlanCanceledEvent(
  Guid PlanId
) : DomainEvent {
  public override Guid AggregateId => PlanId;
}
