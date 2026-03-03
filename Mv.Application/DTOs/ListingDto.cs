using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record ListingDto(
  Guid Id,
  MovieDto? Movie,
  List<ShowtimeDto> Showtimes
) : IdDto(Id);
