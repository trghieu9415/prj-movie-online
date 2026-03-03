using Domain.Enums;
using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record OrderDto(
  Guid Id,
  Guid CustomerId,
  string CustomerName,
  string AuditoriumName,
  OrderStatus Status,
  decimal TotalPrice,
  DateTime CreatedAt,
  List<TicketDto> Tickets
) : IdDto(Id);
