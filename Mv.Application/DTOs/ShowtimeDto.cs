using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record ShowtimeDto(
  Guid Id,
  Guid AuditoriumId,
  DateOnly Date,
  TimeSpan StartAt,
  TimeSpan EndAt
) : IdDto(Id);
