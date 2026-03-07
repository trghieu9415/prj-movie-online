using Domain.Base.Event;
using Domain.Enums;

namespace Domain.Events;

public record PaymentRefundedEvent(
  Guid PaymentId,
  Guid OrderId,
  decimal Amount,
  PaymentMethod Method,
  string? TransactionId
) : DomainEvent {
  public override Guid AggregateId => PaymentId;
}
