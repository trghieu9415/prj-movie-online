using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record AuditoriumDto : IdDto {
  public string Name { get; init; } = null!;
  public List<SeatDto> Seats { get; init; } = [];
}
