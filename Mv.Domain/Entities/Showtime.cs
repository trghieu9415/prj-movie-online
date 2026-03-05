using Domain.Base;

namespace Domain.Entities;

public class Showtime : BaseEntity {
  private Showtime() {}
  public Guid AuditoriumId { get; private set; }
  public DateOnly Date { get; private set; }
  public TimeSpan StartAt { get; private set; }
  public TimeSpan EndAt { get; private set; }

  public static Showtime Create(
    Listing listing,
    Guid auditoriumId,
    DateOnly date, TimeSpan startAt, TimeSpan endAt
  ) {
    var showtime = new Showtime {
      AuditoriumId = auditoriumId,
      Date = date,
      StartAt = startAt
    };
    return showtime;
  }

  public Showtime Update(Guid auditoriumId, DateOnly date, TimeSpan startAt, TimeSpan endAt) {
    AuditoriumId = auditoriumId;
    Date = date;
    StartAt = startAt;
    EndAt = endAt;
    return this;
  }
}
