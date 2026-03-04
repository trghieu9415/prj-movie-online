using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record SeatDto : IdDto {
  public char Row { get; init; }
  public int Number { get; init; }
}
