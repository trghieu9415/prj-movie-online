using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mv.Application.Models;
using Mv.Application.UseCases.Auth.ChangePassword;
using Mv.Application.UseCases.Auth.GetProfile;
using Mv.Application.UseCases.Auth.Login;
using Mv.Application.UseCases.Auth.Refresh;
using Mv.Application.UseCases.Auth.Register;
using Mv.Application.UseCases.Auth.RequestPassword;
using Mv.Application.UseCases.Auth.ResetPassword;
using Mv.Presentation.Controllers.Forms;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

public class AuthController : UserController {
  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginForm form) {
    var command = new LoginCommand(form.Email, form.Password, UserRole.Customer);
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpPost("refresh-token")]
  public async Task<IActionResult> Refresh(RefreshCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [Authorize]
  [HttpGet("profile")]
  public async Task<IActionResult> GetProfile() {
    return AppResponse.Success(await Mediator.Send(new GetProfileQuery()));
  }

  [Authorize]
  [HttpPost("change-password")]
  public async Task<IActionResult> ChangePassword(ChangePasswordCommand command) {
    return AppResponse.Success(await Mediator.Send(command));
  }

  [HttpPost("forgot-password")]
  public async Task<IActionResult> ForgotPassword(RequestPasswordCommand command) {
    return AppResponse.Success(await Mediator.Send(command), "Yêu cầu đã được gửi. Vui lòng kiểm tra email.");
  }

  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPassword(ResetPasswordCommand command) {
    return AppResponse.Success(await Mediator.Send(command), "Mật khẩu đã được đặt lại thành công.");
  }
}
