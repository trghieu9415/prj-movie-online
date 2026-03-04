using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mv.Application.Models;
using Mv.Application.UseCases.Auth.ChangePassword;
using Mv.Application.UseCases.Auth.GetProfile;
using Mv.Application.UseCases.Auth.Login;
using Mv.Application.UseCases.Auth.Refresh;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class AuthController : DashboardController {
  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password) {
    var command = new LoginCommand(email, password, UserRole.Admin);
    return AppResponse.Success(await Mediator.Send(command));
  }

  [AllowAnonymous]
  [HttpPost("refresh-token")]
  public async Task<IActionResult> Refresh(RefreshCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpGet("profile")]
  public async Task<IActionResult> GetProfile() {
    return AppResponse.Success(await Mediator.Send(new GetProfileQuery()));
  }

  [HttpPost("change-password")]
  public async Task<IActionResult> ChangePassword(ChangePasswordCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }
}
