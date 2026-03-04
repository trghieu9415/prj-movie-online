using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record ListingDto : IdDto {
  public MovieDto? Movie { get; init; }
  public List<ShowtimeDto> Showtimes { get; init; } = [];
}
