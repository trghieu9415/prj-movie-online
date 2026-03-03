using Domain.Entities;
using Mv.Application.Constants;
using Mv.Application.DTOs;
using Mv.Application.Ports.Cache;
using Mv.Application.Repositories;
using Mv.Application.Repositories.Read;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Adapters.Cache;

public class BusinessCache(
  ICacheService cache,
  IReadRepository<Auditorium, AuditoriumDto> auditoriumRepository,
  IListingReadRepository listingRepository,
  IPlanReadRepository planRepository
) : IBusinessCache {
  public Task<PlanDto?> GetCurrentPlanAsync(CancellationToken ct) {
    return GetOrSetAsync(
      CacheTags.CurrentPlan,
      async () => await planRepository.GetCurrentPlanAsync(ct),
      TimeSpan.FromDays(7),
      ct
    );
  }

  public Task<AuditoriumDto?> GetAuditoriumAsync(Guid id, CancellationToken ct) {
    return GetOrSetAsync(
      CacheTags.Auditorium(id),
      () => auditoriumRepository.GetByIdAsync(id, ct),
      TimeSpan.FromDays(30),
      ct
    );
  }

  public Task<ListingDto?> GetListingAsync(Guid id, CancellationToken ct) {
    return GetOrSetAsync(
      CacheTags.Listing(id),
      () => listingRepository.GetByIdAsync(id, ct),
      TimeSpan.FromDays(1),
      ct
    );
  }

  // NOTE: ========== [Helper] ==========
  private async Task<T?> GetOrSetAsync<T>(
    string key,
    Func<Task<T?>> fetchLogic,
    TimeSpan expiration,
    CancellationToken ct
  ) {
    var cachedData = await cache.GetAsync<T>(key, ct);
    if (cachedData != null) {
      return cachedData;
    }

    var data = await fetchLogic();
    if (data != null) {
      await cache.SetAsync(key, data, expiration, ct);
    }

    return data;
  }
}
