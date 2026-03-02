using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.CancelOrder;

public record CancelOrderCommand(Guid Id) : ICommand<bool>;
