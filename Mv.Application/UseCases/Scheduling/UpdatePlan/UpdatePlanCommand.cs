using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.UpdatePlan;

public record UpdatePlanCommand(Guid Id, string? Name, int Year, int Month, int Week) : ICommand<bool>;

public class UpdatePlanValidator : AbstractValidator<UpdatePlanCommand> {
  public UpdatePlanValidator() {
    RuleFor(x => x.Id).NotEmpty();
    RuleFor(x => x.Year).GreaterThan(0);
    RuleFor(x => x.Month).InclusiveBetween(1, 12);
    RuleFor(x => x.Week).InclusiveBetween(1, 4);
  }
}
