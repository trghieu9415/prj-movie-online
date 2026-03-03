using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Scheduling.CreatePlan;
using Mv.Application.UseCases.Scheduling.GetPlans;
using Mv.Application.UseCases.Scheduling.SyncListing;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class PlanController : DashboardController {
  [HttpGet]
  public async Task<IActionResult> Get([FromQuery] GetPlansQuery query) {
    return AppResponse.Success(await Mediator.Send(query));
  }

  [HttpPost]
  public async Task<IActionResult> Create(CreatePlanCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpPost("{id:guid}/sync-movies")]
  public async Task<IActionResult> SyncMovies(Guid id, [FromBody] List<Guid> movieIds) {
    return AppResponse.Success(await Mediator.Send(new SyncListingCommand(id, movieIds)));
  }
}
