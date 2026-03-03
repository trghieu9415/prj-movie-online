using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.ProcessPayment;

public record ProcessPaymentCommand(Guid Id, Guid UserId, object Payload) : ICommand<bool>;
