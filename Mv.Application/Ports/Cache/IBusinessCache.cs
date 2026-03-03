using Mv.Application.DTOs;

namespace Mv.Application.Ports.Cache;

public interface IBusinessCache {
  Task<PlanDto?> GetCurrentPlanAsync(CancellationToken ct);
  Task<AuditoriumDto?> GetAuditoriumAsync(Guid id, CancellationToken ct);
  Task<ListingDto?> GetListingAsync(Guid id, CancellationToken ct);
}
