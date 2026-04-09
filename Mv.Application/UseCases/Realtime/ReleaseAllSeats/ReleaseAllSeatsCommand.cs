using MediatR;

namespace Mv.Application.UseCases.Realtime.ReleaseAllSeats;

public record ReleaseAllSeatsCommand(Guid ShowtimeId, Guid UserId) : IRequest<bool>;
