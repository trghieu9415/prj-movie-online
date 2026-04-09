using MediatR;
using Mv.Application.Ports.Realtime;
using Mv.Application.Ports.State;

namespace Mv.Application.UseCases.Realtime.HoldSeat;

public class HoldSeatHandler(
  ISeatStateStore seatStateStore,
  IShowtimeNotifier showtimeNotifier
) : IRequestHandler<HoldSeatCommand, bool> {
  public async Task<bool> Handle(HoldSeatCommand request, CancellationToken ct) {
    await seatStateStore.HoldSeatAsync(request.ShowtimeId, request.UserId, request.SeatId, ct);
    await showtimeNotifier.NotifySeatHeldAsync(request.ShowtimeId, request.SeatId, ct);
    return true;
  }
}
