using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.RefundPayment;

public record RefundPaymentCommand(Guid Id) : ICommand<bool>;
