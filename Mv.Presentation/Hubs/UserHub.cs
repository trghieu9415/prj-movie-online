using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Mv.Presentation.Hubs;

[Authorize]
public class UserHub : Hub;
