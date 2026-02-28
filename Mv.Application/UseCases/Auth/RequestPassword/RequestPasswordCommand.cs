using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Auth.RequestPassword;

public record RequestPasswordCommand(string Email) : ICommand<bool>;

public class RequestPasswordValidator : AbstractValidator<RequestPasswordCommand> {
  public RequestPasswordValidator() {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
  }
}
