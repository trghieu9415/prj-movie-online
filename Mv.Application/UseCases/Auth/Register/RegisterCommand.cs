using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Auth.Register;

public record RegisterCommand(string FullName, string Email, string Password)
  : ITransactional, ICommand<RegisterResult>;

public class RegisterValidator : AbstractValidator<RegisterCommand> {
  public RegisterValidator() {
    RuleFor(x => x.FullName)
      .NotEmpty().WithMessage("Họ tên không được để trống.")
      .MaximumLength(100).WithMessage("Họ tên không được vượt quá 100 ký tự.");

    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email không được để trống.")
      .EmailAddress().WithMessage("Email không đúng định dạng.");

    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("Mật khẩu không được để trống.")
      .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
      .Matches(@"[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ hoa.")
      .Matches(@"[a-z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ thường.")
      .Matches(@"[0-9]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ số.");
  }
}

public record RegisterResult(AuthTokens AuthTokens);
