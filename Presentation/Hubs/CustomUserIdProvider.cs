using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Presentation.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            //return connection.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return connection.User?.FindFirstValue("uid");

        }
    }
}
