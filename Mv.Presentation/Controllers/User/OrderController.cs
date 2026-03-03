using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Booking.GetHistory;
using Mv.Application.UseCases.Booking.GetOrder;
using Mv.Application.UseCases.Booking.PlaceOrder;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

[Authorize]
public class OrderController : UserController {
  [HttpPost]
  public async Task<IActionResult> PlaceOrder(PlaceOrderCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpGet("history")]
  public async Task<IActionResult> History([FromQuery] GetHistoryQuery query) {
    return AppResponse.Success(await Mediator.Send(query));
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> Details(Guid id) {
    return AppResponse.Success(await Mediator.Send(new GetOrderQuery(id)));
  }
}
