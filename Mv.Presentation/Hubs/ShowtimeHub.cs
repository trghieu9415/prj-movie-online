using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Mv.Application.Ports.State;
using Mv.Application.Repositories.Read;
using Mv.Application.UseCases.Realtime.HoldSeat;
using Mv.Application.UseCases.Realtime.ReleaseAllSeats;
using Mv.Application.UseCases.Realtime.ReleaseSeat;

namespace Mv.Presentation.Hubs;

[Authorize]
public class ShowtimeHub(
  IMediator mediator,
  ISeatStateStore seatStateStore,
  IShowtimeReadRepository showtimeReadRepository
) : Hub {
  private static int _userCount;
  private static readonly ConcurrentDictionary<string, Guid> Sessions = new();

  public async Task JoinShowtimeGroup(Guid showtimeId) {
    var showtime = showtimeId.ToString();
    await Groups.AddToGroupAsync(Context.ConnectionId, showtime);
    Sessions[Context.ConnectionId] = showtimeId;

    var bookedSeats = await showtimeReadRepository.GetLockedSeatIdsAsync(showtimeId, Context.ConnectionAborted);
    var heldSeats = await seatStateStore.GetAllHeldSeatsAsync(showtimeId, Context.ConnectionAborted);

    var seatInfo = new { BookedSeats = bookedSeats, HeldSeats = heldSeats };
    await Clients.Caller.SendAsync("SeatStatuses", seatInfo);

    Interlocked.Increment(ref _userCount);
    await Clients.Group(showtime).SendAsync("NewUserJoined", _userCount);
  }

  public override async Task OnDisconnectedAsync(Exception? exception) {
    if (Sessions.TryRemove(Context.ConnectionId, out var showtimeId)) {
      if (Guid.TryParse(Context.UserIdentifier, out var userId)) {
        await mediator.Send(new ReleaseAllSeatsCommand(showtimeId, userId));
      }

      Interlocked.Decrement(ref _userCount);
      await Clients.OthersInGroup(showtimeId.ToString()).SendAsync("NewUserOut", _userCount);
    }

    await base.OnDisconnectedAsync(exception);
  }

  public async Task HoldSeat(Guid seatId) {
    var ct = Context.ConnectionAborted;
    _ = Guid.TryParse(Context.UserIdentifier, out var userId);
    if (Sessions.TryGetValue(Context.ConnectionId, out var showtimeId)) {
      await mediator.Send(new HoldSeatCommand(showtimeId, userId, seatId), ct);
    }
  }

  public async Task ReleaseSeat(Guid seatId) {
    var ct = Context.ConnectionAborted;
    _ = Guid.TryParse(Context.UserIdentifier, out var userId);
    if (Sessions.TryGetValue(Context.ConnectionId, out var showtimeId)) {
      await mediator.Send(new ReleaseSeatCommand(showtimeId, userId, seatId), ct);
    }
  }
}
