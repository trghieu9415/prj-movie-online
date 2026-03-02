using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.RemovePlan;

public record RemovePlanCommand(Guid Id) : ICommand<bool>;
