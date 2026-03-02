namespace Domain.ValueObjects;

public record ShowtimeSnapshot(
  Guid? Id,
  Guid AuditoriumId,
  DayOfWeek DayOfWeek,
  TimeSpan StartAt,
  TimeSpan EndAt
);
