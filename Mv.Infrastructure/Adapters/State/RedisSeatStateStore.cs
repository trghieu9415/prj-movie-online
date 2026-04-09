using Mv.Application.Ports.State;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Adapters.State;

public class RedisSeatStateStore(ICacheService cacheService) : ISeatStateStore {
  public async Task HoldSeatAsync(Guid showtimeId, Guid userId, Guid seatId, CancellationToken ct = default) {
    var key = GetKey(showtimeId);
    var heldSeats = await cacheService.GetAsync<Dictionary<Guid, Guid>>(key, ct) ?? new Dictionary<Guid, Guid>();

    heldSeats[seatId] = userId;
    await cacheService.SetAsync(key, heldSeats, TimeSpan.FromHours(4), ct);
  }

  public async Task ReleaseSeatAsync(Guid showtimeId, Guid userId, Guid seatId, CancellationToken ct = default) {
    var key = GetKey(showtimeId);
    var heldSeats = await cacheService.GetAsync<Dictionary<Guid, Guid>>(key, ct);

    if (heldSeats != null && heldSeats.ContainsKey(seatId) && heldSeats[seatId] == userId) {
      heldSeats.Remove(seatId);
      await cacheService.SetAsync(key, heldSeats, TimeSpan.FromHours(4), ct);
    }
  }

  public async Task ReleaseAllSeatsAsync(Guid showtimeId, Guid userId, CancellationToken ct = default) {
    var key = GetKey(showtimeId);
    var heldSeats = await cacheService.GetAsync<Dictionary<Guid, Guid>>(key, ct);

    if (heldSeats != null) {
      var userSeats = heldSeats.Where(x => x.Value == userId).Select(x => x.Key).ToList();
      foreach (var seat in userSeats) {
        heldSeats.Remove(seat);
      }

      await cacheService.SetAsync(key, heldSeats, TimeSpan.FromHours(4), ct);
    }
  }

  public async Task<List<Guid>> GetAllHeldSeatsAsync(Guid showtimeId, CancellationToken ct = default) {
    var key = GetKey(showtimeId);
    var heldSeats = await cacheService.GetAsync<Dictionary<Guid, Guid>>(key, ct);
    return heldSeats?.Keys.ToList() ?? [];
  }

  public async Task<List<Guid>> GetHeldSeatsByUserAsync(Guid showtimeId, Guid userId, CancellationToken ct = default) {
    var key = $"showtime:{showtimeId}:held_seats";
    var heldSeats = await cacheService.GetAsync<Dictionary<Guid, Guid>>(key, ct);

    return heldSeats == null ? [] : heldSeats.Where(x => x.Value == userId).Select(x => x.Key).ToList();
  }

  private static string GetKey(Guid showtimeId) {
    return $"showtime:{showtimeId}:held_seats";
  }
}
