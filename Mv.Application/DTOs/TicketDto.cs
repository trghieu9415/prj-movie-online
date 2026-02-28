using Domain.ValueObjects;

namespace Mv.Application.DTOs;

public record TicketDto(Guid Id, SeatSnapshot SeatSnapshot, decimal Price);
