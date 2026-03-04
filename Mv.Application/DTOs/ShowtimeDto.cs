using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record ShowtimeDto : IdDto {
  public Guid AuditoriumId { get; init; }
  public DateOnly Date { get; init; }
  public TimeSpan StartAt { get; init; }
  public TimeSpan EndAt { get; init; }
}
