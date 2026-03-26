using Mv.Domain.Base;
using Mv.Domain.ValueObjects;

namespace Mv.Domain.Entities;

public class Listing : BaseEntity {
  private readonly List<Showtime> _showtimes = [];
  private Listing() {}

  public Guid MovieId { get; private set; }
  public IReadOnlyCollection<Showtime> Showtimes => _showtimes.AsReadOnly();

  public static Listing Create(Guid movieId) {
    return new Listing {
      MovieId = movieId
    };
  }

  public void SyncShowtimes(ICollection<ShowtimeSnapshot> showtimes) {
    var incomingIds = showtimes.Where(s => s.Id.HasValue).Select(s => s.Id!.Value).ToList();

    // Remove existing showtimes not in the incoming list of IDs
    _showtimes.RemoveAll(existing => !incomingIds.Contains(existing.Id));

    foreach (var incoming in showtimes) {
      if (!incoming.Id.HasValue) {
        // Null ID means add new
        _showtimes.Add(Showtime.Create(
          this,
          incoming.AuditoriumId,
          incoming.Date,
          incoming.StartAt,
          incoming.EndAt
        ));
      } else {
        // Existing ID, update it
        var existing = _showtimes.FirstOrDefault(s => s.Id == incoming.Id.Value);
        existing?.Update(
          incoming.AuditoriumId,
          incoming.Date,
          incoming.StartAt,
          incoming.EndAt
        );
      }
    }
  }
}
