using Domain.Base;
using Domain.Base.Event;

namespace Domain.Entities;

public class Plan : BaseEntity, IHasDomainEvent {
  private readonly List<Showtime> _showtimes = [];
  private Plan() {}
  public IReadOnlyCollection<Showtime> Showtimes => _showtimes.AsReadOnly();

  public string Name { get; private set; } = null!;
  public int Year { get; private set; }
  public int Month { get; private set; }
  public int Week { get; private set; }

  public static Plan Create(string? name, int year, int month, int week) {
    var plan = new Plan {
      Name = name ?? $"Lịch chiếu Tháng {month} tuần {week} Năm {year}.",
      Year = year,
      Month = month,
      Week = week
    };
    return plan;
  }

  public void SyncShowtimes(ICollection<Showtime> showtimes) {
    _showtimes.Clear();
    _showtimes.AddRange(showtimes);
  }
}
