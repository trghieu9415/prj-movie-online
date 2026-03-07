using Microsoft.AspNetCore.Mvc;
using Mv.Infrastructure.Services.Abstractions;
using Mv.Presentation.Controllers.Forms;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class ExternalController(
  IStorageService storageService
) : DashboardController {
  [HttpPost("upload-image")]
  public async Task<IActionResult> UploadImage([FromForm] ImageFormWithType form, CancellationToken ct = default) {
    if (form.Image == null) {
      return BadRequest("No file uploaded");
    }

    var stream = form.Image.OpenReadStream();
    var extension = Path.GetExtension(form.Image.FileName);

    var folder = form.Type == ImageType.MoviePoster ? "posters" : "profile";

    var url = await storageService.UploadAsync($"{folder}_{Guid.NewGuid()}", stream, extension, folder, ct);
    return AppResponse.Success(new { Url = url });
  }
}
