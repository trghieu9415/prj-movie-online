using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mv.Application.Models;

namespace Mv.Presentation.Controllers;

[Authorize(Roles = nameof(UserRole.Admin))]
[Route("api/dashboard/[controller]")]
[ApiExplorerSettings(GroupName = "v2")]
public abstract class DashboardController : BaseController;
