using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Facility.AddAuditorium;
using Mv.Application.UseCases.Facility.GetAuditorium;
using Mv.Application.UseCases.Facility.GetAuditoriums;
using Mv.Application.UseCases.Facility.RemoveAuditorium;
using Mv.Application.UseCases.Facility.UpdateAuditorium;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class AuditoriumController : DashboardController {
  [HttpGet]
  public async Task<IActionResult> Get([FromQuery] GetAuditoriumsQuery query) {
    var result = await Mediator.Send(query);
    return AppResponse.Success(result.Auditoriums, result.Meta);
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id) {
    return AppResponse.Success(await Mediator.Send(new GetAuditoriumQuery(id)));
  }

  [HttpPost]
  public async Task<IActionResult> Create(AddAuditoriumCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, UpdateAuditoriumCommand command) {
    return AppResponse.Success(await Mediator.Send(command with { Id = id }));
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id) {
    return AppResponse.Success(await Mediator.Send(new RemoveAuditoriumCommand(id)));
  }
}
