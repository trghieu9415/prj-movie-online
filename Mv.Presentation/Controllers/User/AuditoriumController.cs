using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Facility.GetAuditorium;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

public class AuditoriumController : UserController {
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id) {
    var result = await Mediator.Send(new GetAuditoriumQuery(id));
    return AppResponse.Success(result);
  }
}
