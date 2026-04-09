using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Auth.ResetPassword;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : ITransactional, ICommand<bool>;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand> {
  public ResetPasswordValidator() {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email không được để trống.")
      .EmailAddress().WithMessage("Email không đúng định dạng.");

    RuleFor(x => x.Token)
      .NotEmpty().WithMessage("Token không được để trống.")
      .MinimumLength(10).WithMessage("Token không hợp lệ.");

    RuleFor(x => x.NewPassword)
      .NotEmpty().WithMessage("Mật khẩu mới không được để trống.")
      .MinimumLength(6).WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự.")
      .Matches(@"[A-Z]").WithMessage("Mật khẩu mới phải chứa ít nhất 1 chữ hoa.")
      .Matches(@"[a-z]").WithMessage("Mật khẩu mới phải chứa ít nhất 1 chữ thường.")
      .Matches(@"[0-9]").WithMessage("Mật khẩu mới phải chứa ít nhất 1 chữ số.");
  }
}
