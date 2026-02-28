using Domain.ValueObjects;
namespace Mv.Application.DTOs;
public record ShowtimeDto(Guid Id, MovieSnapshot MovieSnapshot, DayOfWeek DayOfWeek, TimeSpan StartAt, TimeSpan EndAt);
