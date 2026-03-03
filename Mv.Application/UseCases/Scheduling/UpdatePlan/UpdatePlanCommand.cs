using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.UpdatePlan;

public record UpdatePlanCommand(Guid Id, string? Name, DateOnly StartDate, DateOnly EndDate) : ICommand<bool>;

public class UpdatePlanValidator : AbstractValidator<UpdatePlanCommand> {
  public UpdatePlanValidator() {
    RuleFor(x => x.Id).NotEmpty();
  }
}
