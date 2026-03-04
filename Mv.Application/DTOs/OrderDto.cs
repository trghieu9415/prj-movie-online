using Domain.Enums;
using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record OrderDto : IdDto {
  public Guid CustomerId { get; init; }
  public string CustomerName { get; init; } = null!;
  public string AuditoriumName { get; init; } = null!;
  public OrderStatus Status { get; init; }
  public decimal TotalPrice { get; init; }
  public DateTime CreatedAt { get; init; }
  public List<TicketDto> Tickets { get; init; } = [];
}
