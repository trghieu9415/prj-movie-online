using Domain.Enums;

namespace Mv.Application.DTOs;

public record OrderDto(
  Guid Id,
  string CustomerName,
  string AuditoriumName,
  OrderStatus Status,
  decimal TotalPrice,
  DateTime CreatedAt,
  List<TicketDto> Tickets
);
