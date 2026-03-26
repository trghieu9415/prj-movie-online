using Mv.Domain.Base;
using Mv.Domain.Enums;
using Mv.Domain.Events;
using Mv.Domain.Exceptions;

namespace Mv.Domain.Entities;

public class Plan : BaseEntity {
  private readonly List<Listing> _listings = [];
  public PlanStatus Status = PlanStatus.Draft;
  private Plan() {}
  public IReadOnlyCollection<Listing> Listings => _listings.AsReadOnly();

  public string Name { get; private set; } = null!;
  public DateOnly StartDate { get; private set; }
  public DateOnly EndDate { get; private set; }

  public static Plan Create(string? name, DateOnly startDate, DateOnly endDate) {
    var plan = new Plan {
      Name = name ?? $"Lịch chiếu từ {startDate} - {endDate}.",
      StartDate = startDate,
      EndDate = endDate
    };
    return plan;
  }

  public Plan Update(string? name, DateOnly startDate, DateOnly endDate) {
    Name = name ?? $"Lịch chiếu từ {startDate} - {endDate}.";
    StartDate = startDate;
    EndDate = endDate;
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
  }

  public void Cancel() {
    if (Status != PlanStatus.Draft) {
      throw new DomainException("Chỉ có thể hủy lịch chiếu ở trạng thái Công khai");
    }

    Status = PlanStatus.Canceled;
    AddDomainEvent(new PlanCanceledEvent(Id));
  }
}
