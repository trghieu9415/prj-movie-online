using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.CreatePlan;

public record CreatePlanCommand(string? Name, DateOnly StartDate, DateOnly EndDate) : ICommand<Guid>;

public class CreatePlanValidator : AbstractValidator<CreatePlanCommand> {}
