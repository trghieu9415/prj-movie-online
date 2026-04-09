using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Auth.ChangePassword;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : ITransactional, ICommand<bool>;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand> {
  public ChangePasswordValidator() {
    RuleFor(x => x.OldPassword)
      .NotEmpty().WithMessage("Mật khẩu cũ không được để trống.")
      .MinimumLength(6).WithMessage("Mật khẩu cũ phải có ít nhất 6 ký tự.");

    RuleFor(x => x.NewPassword)
      .NotEmpty().WithMessage("Mật khẩu mới không được để trống.")
      .MinimumLength(6).WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự.")
      .NotEqual(x => x.OldPassword).WithMessage("Mật khẩu mới không được trùng mật khẩu cũ.");

    RuleFor(x => x.NewPassword)
      .Matches(@"[A-Z]").WithMessage("Mật khẩu mới phải chứa ít nhất 1 chữ hoa.")
      .Matches(@"[a-z]").WithMessage("Mật khẩu mới phải chứa ít nhất 1 chữ thường.")
      .Matches(@"[0-9]").WithMessage("Mật khẩu mới phải chứa ít nhất 1 chữ số.");
  }
}
