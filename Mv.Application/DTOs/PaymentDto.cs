using Domain.Enums;

namespace Mv.Application.DTOs;

public record PaymentDto(
  Guid Id,
  Guid OrderId,
  decimal Amount,
  string? TransactionId,
  PaymentMethod Method,
  PaymentStatus Status,
  DateTime CreatedAt
);
