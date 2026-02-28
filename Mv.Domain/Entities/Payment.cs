using Domain.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Payment : BaseEntity {
  private Payment() {}

  public Guid OrderId { get; private set; }
  public decimal Amount { get; private set; }
  public string? TransactionId { get; private set; }
  public PaymentMethod Method { get; private set; }
  public PaymentStatus Status { get; private set; }

  public static Payment Create(Guid orderId, decimal amount, PaymentMethod method) {
    return new Payment {
      OrderId = orderId,
      Amount = amount,
      Method = method,
      Status = PaymentStatus.Pending
    };
  }

  public void MarkAsSucceeded(string transactionId) {
    TransactionId = transactionId;
    Status = PaymentStatus.Succeeded;
  }

  public void MarkAsFailed() {
    Status = PaymentStatus.Failed;
  }

  public void Refund() {
    Status = PaymentStatus.Refunded;
  }
}
