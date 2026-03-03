namespace Domain.ValueObjects;

public record ShowtimeSnapshot(
  Guid? Id,
  Guid AuditoriumId,
  DateOnly Date,
  TimeSpan StartAt,
  TimeSpan EndAt
);
