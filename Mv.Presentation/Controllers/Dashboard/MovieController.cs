using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Catalog.AddMovie;
using Mv.Application.UseCases.Catalog.GetMovies;
using Mv.Application.UseCases.Catalog.RemoveMovie;
using Mv.Application.UseCases.Catalog.UpdateMovie;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class MovieController : DashboardController {
  [HttpGet]
  public async Task<IActionResult> Get([FromQuery] GetMoviesQuery query) {
    var result = await Mediator.Send(query);
    return AppResponse.Success(result.Movies, result.Meta);
  }

  [HttpPost]
  public async Task<IActionResult> Create(AddMovieCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, UpdateMovieCommand command) {
    return AppResponse.Success(await Mediator.Send(command with { Id = id }));
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id) {
    return AppResponse.Success(await Mediator.Send(new RemoveMovieCommand(id)));
  }
}
