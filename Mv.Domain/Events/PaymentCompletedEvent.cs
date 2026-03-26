using Mv.Domain.Base.Event;
using Mv.Domain.Enums;

namespace Mv.Domain.Events;

public record PaymentCompletedEvent(
  Guid PaymentId,
  Guid OrderId,
  Guid CustomerId,
  decimal Amount,
  PaymentMethod Method,
  string? TransactionId
) : DomainEvent {
  public override Guid AggregateId => PaymentId;
}
