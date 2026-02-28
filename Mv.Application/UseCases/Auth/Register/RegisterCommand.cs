using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Auth.Register;

public record RegisterCommand(string FullName, string Email, string Password) : ICommand<RegisterResult>;

public class RegisterValidator : AbstractValidator<RegisterCommand> {
  public RegisterValidator() {
    RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
  }
}

public record RegisterResult(AuthTokens AuthTokens);
