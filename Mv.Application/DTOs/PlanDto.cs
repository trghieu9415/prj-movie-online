using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record PlanDto : IdDto {
  public string Name { get; init; } = null!;
  public DateOnly StartDate { get; init; }
  public DateOnly EndDate { get; init; }
  public List<ListingDto> Listings { get; init; } = [];
}
