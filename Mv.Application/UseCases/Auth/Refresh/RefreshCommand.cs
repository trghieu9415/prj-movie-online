using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Auth.Refresh;

public record RefreshCommand(string RefreshToken) : ITransactional, ICommand<RefreshResult>;

public record RefreshResult(AuthTokens AuthTokens);

public class RefreshValidator : AbstractValidator<RefreshCommand> {
  public RefreshValidator() {
    RuleFor(x => x.RefreshToken)
      .NotEmpty().WithMessage("Refresh token không được để trống.")
      .MinimumLength(10).WithMessage("Refresh token không hợp lệ.");
  }
}
