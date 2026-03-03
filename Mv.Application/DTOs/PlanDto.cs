using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record PlanDto(
  Guid Id,
  string Name,
  DateOnly StartDate,
  DateOnly EndDate,
  List<ListingDto> Listings
) : IdDto(Id);
