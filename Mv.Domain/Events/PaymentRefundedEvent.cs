using Mv.Domain.Base.Event;
using Mv.Domain.Enums;

namespace Mv.Domain.Events;

public record PaymentRefundedEvent(
  Guid PaymentId,
  Guid OrderId,
  decimal Amount,
  PaymentMethod Method,
  string? TransactionId
) : DomainEvent {
  public override Guid AggregateId => PaymentId;
}
