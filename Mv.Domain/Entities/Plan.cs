using Domain.Base;

namespace Domain.Entities;

public class Plan : BaseEntity {
  private readonly List<Listing> _listings = [];
  private Plan() { }
  public IReadOnlyCollection<Listing> Listings => _listings.AsReadOnly();

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

  public Plan Update(string? name, int year, int month, int week) {
    Name = name ?? $"Lịch chiếu Tháng {month} tuần {week} Năm {year}.";
    Year = year;
    Month = month;
    Week = week;
    return this;
  }

  public void SyncListings(ICollection<Guid> movieIds) {
    // Delta remove
    _listings.RemoveAll(l => !movieIds.Contains(l.MovieId));

    // Delta add
    var existingMovieIds = _listings.Select(l => l.MovieId).ToHashSet();
    var toAdd = movieIds.Where(id => !existingMovieIds.Contains(id))
      .Select(id => Listing.Create(id))
      .ToList();
    _listings.AddRange(toAdd);
  }
}
