namespace Mv.Application.Ports.Notification;

public interface IEmailService {
  Task SendEmailAsync(string email, string subject, string body, CancellationToken ct = default);
  Task SendOrderConfirmationEmailAsync(string email, string orderId, CancellationToken ct = default);
  Task SendResetPasswordEmailAsync(string email, string token, CancellationToken ct = default);
}
