using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Realtime.HoldSeat;

public record HoldSeatCommand(Guid ShowtimeId, Guid UserId, Guid SeatId) : ICommand<bool>;
