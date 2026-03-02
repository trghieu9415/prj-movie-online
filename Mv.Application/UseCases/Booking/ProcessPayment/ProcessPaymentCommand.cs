using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.ProcessPayment;

public record ProcessPaymentCommand(Guid Id, object Payload) : ICommand<bool>;
