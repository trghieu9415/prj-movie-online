using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.CreatePlan;

public record CreatePlanCommand(string? Name, int Year, int Month, int Week) : ICommand<Guid>;

public class CreatePlanValidator : AbstractValidator<CreatePlanCommand> {
  public CreatePlanValidator() {
    RuleFor(x => x.Year).GreaterThan(0);
    RuleFor(x => x.Month).InclusiveBetween(1, 12);
    RuleFor(x => x.Week).InclusiveBetween(1, 53);
  }
}
