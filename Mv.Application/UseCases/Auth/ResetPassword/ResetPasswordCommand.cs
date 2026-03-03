using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Auth.ResetPassword;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : ICommand<bool>;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand> {
  public ResetPasswordValidator() {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.Token).NotEmpty();
    RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
  }
}
