using Microsoft.AspNetCore.Mvc;

namespace Mv.Presentation.Controllers;

[Route("api/user/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public abstract class UserController : BaseController;
