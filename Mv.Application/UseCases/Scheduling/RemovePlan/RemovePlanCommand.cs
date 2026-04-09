using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.RemovePlan;

public record RemovePlanCommand(Guid Id) : ICommand<bool>;

public class RemovePlanValidator : AbstractValidator<RemovePlanCommand> {
  public RemovePlanValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}
