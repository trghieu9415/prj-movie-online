using Microsoft.AspNetCore.Mvc;
using Mv.Infrastructure.Services.Abstractions;
using Mv.Presentation.Controllers.Forms;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

public class ExternalController(
  IStorageService storageService
) : UserController {
  [HttpPost("upload-image")]
  public async Task<IActionResult> UploadImage([FromForm] ImageForm form, CancellationToken ct = default) {
    if (form.Image == null) {
      return BadRequest("No file uploaded");
    }

    var stream = form.Image.OpenReadStream();
    var extension = Path.GetExtension(form.Image.FileName);

    const string folder = "profile";

    var url = await storageService.UploadAsync($"{folder}_{Guid.NewGuid()}", stream, extension, folder, ct);
    return AppResponse.Success(new { Url = url });
  }
}
