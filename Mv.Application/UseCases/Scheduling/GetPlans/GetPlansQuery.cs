using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Scheduling.GetPlans;

public record GetPlansQuery(int Page = 1, int PageSize = 10) : IQuery<GetPlansResult>;

public record GetPlansResult(List<PlanDto> Plans, Meta Meta);
