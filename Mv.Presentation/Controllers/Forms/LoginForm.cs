namespace Mv.Presentation.Controllers.Forms;

public record LoginForm {
  public string Email { get; init; } = "";
  public string Password { get; init; } = "";
}
