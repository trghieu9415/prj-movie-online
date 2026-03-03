using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Scheduling.GetCurrentPlan;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

public class PlanController : UserController {
  [HttpGet("current")]
  public async Task<IActionResult> GetCurrent() {
    var result = await Mediator.Send(new GetCurrentPlanQuery());
    return AppResponse.Success(result);
  }
}
