using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using Mv.Application.Ports.Notification;
using Mv.Infrastructure.Options;

namespace Mv.Infrastructure.Adapters.Notification;

public class EmailService(
  EmailOptions options,
  ILogger<EmailService> logger
) : IEmailService {
  public async Task SendEmailAsync(string email, string subject, string body, CancellationToken ct = default) {
    try {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(options.FromName, options.FromAddress));
      message.To.Add(MailboxAddress.Parse(email));
      message.Subject = subject;

      message.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

      using var client = new SmtpClient();
      await client.ConnectAsync(options.Host, options.Port,
        options.EnableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto, ct);

      if (!string.IsNullOrEmpty(options.Username)) {
        await client.AuthenticateAsync(options.Username, options.Password, ct);
      }

      await client.SendAsync(message, ct);
      await client.DisconnectAsync(true, ct);

      logger.LogInformation("Đã gửi email thành công đến {Email}", email);
    } catch (Exception ex) {
      logger.LogError(ex, "Lỗi khi gửi email đến {Email}", email);
      throw;
    }
  }

  public async Task SendOrderConfirmationEmailAsync(string email, string orderId, CancellationToken ct = default) {
    var subject = $"Xác nhận đơn hàng #{orderId} thành công";

    var htmlBody =
      $"""
        <div style='font-family: Arial, sans-serif; padding: 20px;'>
          <h2>Cảm ơn bạn đã đặt hàng!</h2>
          <p>Đơn hàng mã số <strong>{orderId}</strong> của bạn đã được xác nhận.</p>
          <p>Chúng tôi sẽ sớm cập nhật trạng thái vận chuyển đến bạn.</p>
       </div>
       """;

    await SendEmailAsync(email, subject, htmlBody, ct);
  }

  public async Task SendResetPasswordEmailAsync(string email, string token, CancellationToken ct = default) {
    const string subject = "Yêu cầu khôi phục mật khẩu";

    var resetLink = $"https://sgu-bidding.local/reset-password?token={token}&email={email}";

    var htmlBody =
      $"""
       <div style='font-family: Arial, sans-serif; padding: 20px;'>
         <h2>Khôi phục mật khẩu</h2>
         <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
          <p>Vui lòng click vào nút bên dưới để tiến hành:</p>
         <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Đặt lại mật khẩu</a>
         <p>Link này sẽ hết hạn sau 15 phút.</p>
       </div>
       """;

    await SendEmailAsync(email, subject, htmlBody, ct);
  }
}
