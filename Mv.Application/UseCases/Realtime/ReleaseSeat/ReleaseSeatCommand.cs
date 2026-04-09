using MediatR;

namespace Mv.Application.UseCases.Realtime.ReleaseSeat;

public record ReleaseSeatCommand(Guid ShowtimeId, Guid UserId, Guid SeatId) : IRequest<bool>;
