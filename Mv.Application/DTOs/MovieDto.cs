using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record MovieDto : IdDto {
  public string Name { get; init; } = null!;
  public int Duration { get; init; }
  public string PosterUrl { get; init; } = null!;
}
