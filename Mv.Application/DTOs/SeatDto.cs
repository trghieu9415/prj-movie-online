using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record SeatDto(Guid Id, char Row, int Number) : IdDto(Id);
