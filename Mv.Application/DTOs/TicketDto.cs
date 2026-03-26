using Mv.Application.DTOs.Base;
using Mv.Domain.ValueObjects;

namespace Mv.Application.DTOs;

public record TicketDto : IdDto {
  public SeatSnapshot SeatSnapshot { get; init; } = null!;
  public decimal Price { get; init; }
}
