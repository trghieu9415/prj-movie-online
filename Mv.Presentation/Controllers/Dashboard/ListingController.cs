using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Scheduling.GetListing;
using Mv.Application.UseCases.Scheduling.SyncShowtime;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class ListingController : DashboardController {
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id) {
    return AppResponse.Success(await Mediator.Send(new GetListingQuery(id)));
  }

  [HttpPost("{id:guid}/sync-showtimes")]
  public async Task<IActionResult> SyncShowtimes(Guid id, SyncShowtimeCommand command) {
    var result = await Mediator.Send(command with { Id = id });
    return AppResponse.Success(result, "Cập nhật danh sách suất chiếu thành công");
  }
}
