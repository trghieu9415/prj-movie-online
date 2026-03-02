using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record PlanDto(
  Guid Id,
  string Name,
  int Year,
  int Month,
  int Week,
  List<ListingDto> Listings
) : IdDto(Id);
