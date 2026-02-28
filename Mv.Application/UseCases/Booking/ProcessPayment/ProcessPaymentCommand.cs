using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.ProcessPayment;

public record ProcessPaymentCommand(Guid PaymentId, object Payload) : ICommand<bool>;
