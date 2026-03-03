using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Scheduling.GetListing;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

public class ListingController : UserController {
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id) {
    var result = await Mediator.Send(new GetListingQuery(id));
    return AppResponse.Success(result);
  }
}
