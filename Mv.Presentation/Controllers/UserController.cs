using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mv.Application.Models;

namespace Mv.Presentation.Controllers;

[Authorize(Roles = nameof(UserRole.Customer))]
[Route("api/user/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public abstract class UserController : BaseController;
