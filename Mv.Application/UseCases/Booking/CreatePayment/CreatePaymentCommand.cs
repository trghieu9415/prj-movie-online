using Mv.Application.Abstractions;
using Mv.Domain.Enums;

namespace Mv.Application.UseCases.Booking.CreatePayment;

public record CreatePaymentCommand(Guid OrderId, PaymentMethod Method) : ICommand<CreatePaymentResult>;

public record CreatePaymentResult(Guid PaymentId, string PaymentUrl);
