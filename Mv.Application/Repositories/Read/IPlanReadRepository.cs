using Mv.Application.DTOs;
using Mv.Domain.Entities;

namespace Mv.Application.Repositories.Read;

public interface IPlanReadRepository : IReadRepository<Plan, PlanDto> {
  Task<PlanDto?> GetPlanOverviewAsync(Guid id, CancellationToken ct = default);
  Task<PlanDto?> GetCurrentPlanAsync(CancellationToken ct = default);
}
