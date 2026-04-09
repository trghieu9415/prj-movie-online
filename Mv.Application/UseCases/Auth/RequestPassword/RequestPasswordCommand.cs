using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Auth.RequestPassword;

public record RequestPasswordCommand(string Email) : ITransactional, ICommand<bool>;

public class RequestPasswordValidator : AbstractValidator<RequestPasswordCommand> {
  public RequestPasswordValidator() {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email không được để trống.")
      .EmailAddress().WithMessage("Email không đúng định dạng.");
  }
}
