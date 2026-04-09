using MediatR;
using Mv.Application.Ports.Realtime;
using Mv.Application.Ports.State;

namespace Mv.Application.UseCases.Realtime.ReleaseSeat;

public class ReleaseSeatHandler(
  ISeatStateStore seatStateStore,
  IShowtimeNotifier showtimeNotifier
) : IRequestHandler<ReleaseSeatCommand, bool> {
  public async Task<bool> Handle(ReleaseSeatCommand request, CancellationToken ct) {
    await seatStateStore.ReleaseSeatAsync(request.ShowtimeId, request.UserId, request.SeatId, ct);
    await showtimeNotifier.NotifySeatReleasedAsync(request.ShowtimeId, [request.SeatId], ct);
    return true;
  }
}
