using Mv.Application.DTOs.Base;
using Mv.Domain.Enums;
using Mv.Domain.ValueObjects;

namespace Mv.Application.DTOs;

public record OrderDto : IdDto {
  public Guid CustomerId { get; init; }
  public string CustomerName { get; init; } = null!;
  public string AuditoriumName { get; init; } = null!;
  public OrderStatus Status { get; init; }
  public decimal TotalPrice { get; init; }
  public DateTime CreatedAt { get; init; }
  public MovieSnapshot Movie { get; init; } = null!;
  public List<TicketDto> Tickets { get; init; } = [];
}
