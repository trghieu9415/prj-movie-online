using Domain.Enums;
using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record PaymentDto : IdDto {
  public Guid OrderId { get; init; }
  public decimal Amount { get; init; }
  public string? TransactionId { get; init; }
  public PaymentMethod Method { get; init; }
  public PaymentStatus Status { get; init; }
  public DateTime CreatedAt { get; init; }
}
