using Domain.Entities;

namespace Mv.Application.Ports.Gateway;

public interface IPaymentGateway {
  string CreatePaymentUrl(Payment payment);
  (bool isSucceed, string transactionId) VerifyPayment(PaymentPayload payload);
  bool RefundPayment(Payment payment);

  // Phương thức chuyển hóa đầu vào Payload thành Payload đặc trưng của Method
  PaymentPayload ToPaymentPayload(object objectPayload);
}

public abstract record PaymentPayload;
