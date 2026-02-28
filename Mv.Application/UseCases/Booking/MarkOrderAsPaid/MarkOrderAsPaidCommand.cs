using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.MarkOrderAsPaid;

public record MarkOrderAsPaidCommand(Guid OrderId) : ICommand<bool>;
