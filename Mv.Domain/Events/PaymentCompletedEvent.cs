using Domain.Base.Event;
using Domain.Enums;

namespace Domain.Events;

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
