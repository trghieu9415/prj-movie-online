using Domain.Enums;
using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record PaymentDto(
  Guid Id,
  Guid OrderId,
  decimal Amount,
  string? TransactionId,
  PaymentMethod Method,
  PaymentStatus Status,
  DateTime CreatedAt
) : IdDto(Id);
