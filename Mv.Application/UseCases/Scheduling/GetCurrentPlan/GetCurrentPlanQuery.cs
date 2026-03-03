using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Scheduling.GetCurrentPlan;

public record GetCurrentPlanQuery : IQuery<GetCurrentPlanResult>;

public record GetCurrentPlanResult(PlanDto Plan);
