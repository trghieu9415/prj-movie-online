using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Application.Ports.Repositories.Read;

public interface IPlanReadRepository : IReadRepository<Plan, PlanDto> {
  Task<PlanDto?> GetPlanOverviewAsync(Guid id, CancellationToken ct = default);
}
