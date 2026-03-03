using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Booking.GetOrders;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class OrderController : DashboardController {
  [HttpGet]
  public async Task<IActionResult> Get([FromQuery] GetOrdersQuery query) {
    return AppResponse.Success(await Mediator.Send(query));
  }
}
