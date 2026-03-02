using Domain.ValueObjects;
using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record TicketDto(Guid Id, SeatSnapshot SeatSnapshot, decimal Price) : IdDto(Id);
