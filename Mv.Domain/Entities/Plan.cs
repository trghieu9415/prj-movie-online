using Domain.Base;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;

namespace Domain.Entities;

public class Plan : BaseEntity {
  private readonly List<Listing> _listings = [];
  public PlanStatus Status = PlanStatus.Draft;
  private Plan() {}
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
    _listings.RemoveAll(l => !movieIds.Contains(l.MovieId));

    var existingMovieIds = _listings.Select(l => l.MovieId).ToHashSet();
    var toAdd = movieIds.Where(id => !existingMovieIds.Contains(id))
      .Select(id => Listing.Create(id))
      .ToList();
    _listings.AddRange(toAdd);
  }

  public void Publish() {
    if (Status != PlanStatus.Draft) {
      throw new DomainException("Chỉ có thể công bố lịch chiếu ở trạng thái Dự kiến");
    }

    Status = PlanStatus.Published;
    AddDomainEvent(new PlanPublishedEvent(
      Id,
      Listings.Select(l => l.Id).ToList(),
      Listings.Select(l => l.MovieId).ToList())
    );
  }

  public void Cancel() {
    if (Status != PlanStatus.Draft) {
      throw new DomainException("Chỉ có thể hủy lịch chiếu ở trạng thái Công khai");
    }

    Status = PlanStatus.Canceled;
    AddDomainEvent(new PlanCanceledEvent(Id));
  }
}
