using Microsoft.AspNetCore.Mvc;
using Mv.Application.Ports.Security;
using Mv.Application.UseCases.Scheduling.GetLockedSeatQuery;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

public class ShowtimeController(ICurrentUser currentUser) : UserController {
  [HttpGet("{id:guid}/reserved-seats")]
  public async Task<IActionResult> GetReservedSeats(Guid id) {
    var query = new GetLockedSeatQuery(id);
    return AppResponse.Success(await Mediator.Send(query));
  }
}
