using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Presentation.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {

    }
}
