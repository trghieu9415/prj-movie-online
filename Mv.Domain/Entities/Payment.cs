using Domain.Base;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;

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

  public void MarkAsSucceeded(Guid userId, string transactionId) {
    if (Status != PaymentStatus.Pending) {
      throw new DomainException("Chỉ có thể hoàn tất thanh toán khi thanh toán ở trạng thái chờ");
    }

    TransactionId = transactionId;
    Status = PaymentStatus.Succeeded;
    AddDomainEvent(new PaymentCompletedEvent(Id, OrderId, userId, Amount, Method, TransactionId));
  }

  public void MarkAsFailed() {
    if (Status != PaymentStatus.Pending) {
      throw new DomainException("Chỉ có thể hủy thanh toán khi đang ở trạng thái chờ");
    }

    Status = PaymentStatus.Failed;
  }

  public void Refund() {
    if (Status != PaymentStatus.Succeeded) {
      throw new DomainException("Chỉ có thể hoàn tiền thanh toán với thanh toán đã hoàn tất");
    }

    Status = PaymentStatus.Refunded;
    AddDomainEvent(new PaymentRefundedEvent(Id, OrderId, Amount, Method, TransactionId));
  }
}
