using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Scheduling.GetPlan;

public record GetPlanQuery(Guid Id) : IQuery<GetPlanResult>;

public record GetPlanResult(PlanDto Plan);
