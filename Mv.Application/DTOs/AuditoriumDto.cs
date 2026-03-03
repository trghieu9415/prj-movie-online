using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record AuditoriumDto(Guid Id, string Name, List<SeatDto> Seats) : IdDto(Id);
