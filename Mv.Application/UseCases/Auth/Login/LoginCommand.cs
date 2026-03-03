using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Auth.Login;

public record LoginCommand(string Email, string Password, UserRole Role) : ICommand<LoginResult>;

public class LoginValidator : AbstractValidator<LoginCommand> {
  public LoginValidator() {
    RuleFor(x => x.Email)
      .NotNull()
      .NotEmpty();
    RuleFor(x => x.Password)
      .NotNull()
      .NotEmpty();
  }
}

public record LoginResult(AuthTokens AuthTokens);
