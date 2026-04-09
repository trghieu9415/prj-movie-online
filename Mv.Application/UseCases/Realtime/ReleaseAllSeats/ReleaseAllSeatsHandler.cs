using MediatR;
using Mv.Application.Ports.Realtime;
using Mv.Application.Ports.State;

namespace Mv.Application.UseCases.Realtime.ReleaseAllSeats;

public class ReleaseAllSeatsHandler(
  ISeatStateStore seatStateStore,
  IShowtimeNotifier showtimeNotifier
) : IRequestHandler<ReleaseAllSeatsCommand, bool> {
  public async Task<bool> Handle(ReleaseAllSeatsCommand request, CancellationToken ct) {
    var seatsOfUser = await seatStateStore.GetHeldSeatsByUserAsync(request.ShowtimeId, request.UserId, ct);

    if (seatsOfUser.Count <= 0) {
      return true;
    }

    await seatStateStore.ReleaseAllSeatsAsync(request.ShowtimeId, request.UserId, ct);
    await showtimeNotifier.NotifySeatReleasedAsync(request.ShowtimeId, seatsOfUser, ct);

    return true;
  }
}
