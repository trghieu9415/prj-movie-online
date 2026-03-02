using Domain.Base;

namespace Domain.Entities;

public class Showtime : BaseEntity {
  private Showtime() { }
  public Guid AuditoriumId { get; private set; }
  public DayOfWeek DayOfWeek { get; private set; }
  public TimeSpan StartAt { get; private set; }
  public TimeSpan EndAt { get; private set; }

  public static Showtime Create(
    Guid auditoriumId,
    DayOfWeek dayOfWeek, TimeSpan startAt, TimeSpan endAt
  ) {
    var showtime = new Showtime {
      AuditoriumId = auditoriumId,
      DayOfWeek = dayOfWeek,
      StartAt = startAt,
    };
    return showtime;
  }

  public Showtime Update(Guid auditoriumId, DayOfWeek dayOfWeek, TimeSpan startAt, TimeSpan endAt) {
    AuditoriumId = auditoriumId;
    DayOfWeek = dayOfWeek;
    StartAt = startAt;
    EndAt = endAt;
    return this;
  }
}
