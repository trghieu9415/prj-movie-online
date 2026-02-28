namespace Mv.Application.DTOs;

public record AuditoriumDto(Guid Id, string Name, List<SeatDto> Seats);
