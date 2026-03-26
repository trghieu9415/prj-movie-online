using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Models;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Scheduling.GetPlans;

public class GetPlansHandler(
  IReadRepository<Plan, PlanDto> planReadRepo
) : IRequestHandler<GetPlansQuery, GetPlansResult> {
  public async Task<GetPlansResult> Handle(GetPlansQuery request, CancellationToken ct) {
    var (total, planDtos) = await planReadRepo.GetAsync(
      page: request.Page,
      pageSize: request.PageSize,
      ct: ct
    );
    var meta = Meta.Create(request.Page, request.PageSize, total);
    return new GetPlansResult(planDtos, meta);
  }
}
