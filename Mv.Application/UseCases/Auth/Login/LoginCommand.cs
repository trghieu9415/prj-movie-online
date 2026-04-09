using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Auth.Login;

public record LoginCommand(string Email, string Password, UserRole Role) : ITransactional, ICommand<LoginResult>;

public class LoginValidator : AbstractValidator<LoginCommand> {
  public LoginValidator() {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email không được để trống.")
      .EmailAddress().WithMessage("Email không đúng định dạng.");

    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("Mật khẩu không được để trống.");

    RuleFor(x => x.Role)
      .IsInEnum().WithMessage("Role không hợp lệ.");
  }
}

public record LoginResult(AuthTokens AuthTokens);
