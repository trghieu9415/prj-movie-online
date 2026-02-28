using Domain.Base;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Showtime : BaseEntity {
  private Showtime() {}

  public MovieSnapshot MovieSnapshot { get; private set; } = null!;
  public DayOfWeek DayOfWeek { get; private set; }
  public TimeSpan StartAt { get; private set; }
  public TimeSpan EndAt { get; private set; }

  public static Showtime Create(MovieSnapshot movieSnapshot,
    DayOfWeek dayOfWeek, TimeSpan startAt, TimeSpan endAt
  ) {
    var showtime = new Showtime {
      MovieSnapshot = movieSnapshot,
      DayOfWeek = dayOfWeek,
      StartAt = startAt,
      EndAt = endAt
    };
    return showtime;
  }
}
