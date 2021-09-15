using Microsoft.AspNetCore.SignalR;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Hubs;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Services.PersonalChat
{
    public class ChatAppService : IChatAppService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IPortalUserAppService _portalUserAppService;
        private readonly IUserAppService _adminUserAppService;

        public ChatAppService(IHubContext<ChatHub> hubContext, IPortalUserAppService portalUserAppService)
        {
            _hubContext = hubContext;
            _portalUserAppService = portalUserAppService;
        }

        public async Task JoinUserToChatAsync(int userId, string roomName, string connectionId)
        {
            var user = await _portalUserAppService.GetPortalUserByIdAsync(userId);
            await JoinToChatAsync(user.FirstName + " " + user.LastName, roomName, connectionId);
        }

        public async Task JoinAdminToChatAsync(int userId, string roomName, string connectionId)
        {
            var user = _adminUserAppService.GetAdminById(userId);
            await JoinToChatAsync(user.Name, roomName, connectionId);
        }

        private async Task JoinToChatAsync(string userFullName, string roomName, string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, roomName);
            await _hubContext.Clients.GroupExcept(roomName, connectionId).SendAsync("addUser", userFullName);
        }
    }
}
