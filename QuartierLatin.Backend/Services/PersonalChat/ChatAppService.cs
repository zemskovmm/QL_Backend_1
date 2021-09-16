using Microsoft.AspNetCore.SignalR;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PersonalChatRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels;
using QuartierLatin.Backend.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;
using QuartierLatin.Backend.Dto.PersonalChatDto;

namespace QuartierLatin.Backend.Services.PersonalChat
{
    public class ChatAppService : IChatAppService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IPortalUserAppService _portalUserAppService;
        private readonly IUserAppService _adminUserAppService;
        private readonly IChatRepository _chatRepository;
        private readonly IPortalPersonalAppService _portalPersonalAppService;
        private readonly string _chatPrefix = "personal-chat-";

        public ChatAppService(IHubContext<ChatHub> hubContext, IPortalUserAppService portalUserAppService,
            IUserAppService userAppService, IChatRepository chatRepository, 
            IPortalPersonalAppService portalPersonalAppService)
        {
            _hubContext = hubContext;
            _portalUserAppService = portalUserAppService;
            _adminUserAppService = userAppService;
            _chatRepository = chatRepository;
            _portalPersonalAppService = portalPersonalAppService;
        }

        public async Task JoinUserToChatAsync(int userId, int applicationId, string connectionId)
        {
            var user = await _portalUserAppService.GetPortalUserByIdAsync(userId);

            var response = await _portalPersonalAppService.CheckIsUserOwnerAsync(userId, applicationId);

            if (response is false)
                throw new BadHttpRequestException("Denied");

            await JoinToChatAsync(user.FirstName + " " + user.LastName, applicationId, connectionId);
        }

        public async Task JoinAdminToChatAsync(int userId, int applicationId, string connectionId)
        {
            var user = _adminUserAppService.GetAdminById(userId);
            await JoinToChatAsync("Staff", applicationId, connectionId);
        }

        public async Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int portalUserId)
        {
            return await _chatRepository.GetChatMessagesAsync(applicationId, portalUserId);
        }

        public async Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, MessageType type, string text = null, int? blobId = null)
        {
            var response = await _chatRepository.SendChatMessageAsync(applicationId, portalUserId, type, text, blobId);

            if (response is false)
                return response;

            await SendMessageToGroup(applicationId, type, "User", text);

            return response;
        }

        public async Task<List<ChatMessages>> GetChatMessagesAdminAsync(int applicationId)
        {
            return await _chatRepository.GetChatMessagesAdminAsync(applicationId);
        }

        public async Task<bool> SendChatMessageAdminAsync(int applicationId, MessageType type, string text = null, int? blobId = null)
        {
            var response = await _chatRepository.SendChatMessageAdminAsync(applicationId, type, text, blobId);

            if (response is false)
                return response;

            await SendMessageToGroup(applicationId, type, "Staff", text);

            return response;
        }

        public async Task<List<(Chat chat, PortalUser user)>> GetChatsAsync()
        {
            return await _chatRepository.GetChatsAsync();
        }

        private async Task JoinToChatAsync(string userFullName, int applicationId, string connectionId)
        {
            var groupName = GetGroupName(applicationId);

            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
            await _hubContext.Clients.GroupExcept(groupName, connectionId).SendAsync("addUser", userFullName);
        }

        private string GetGroupName(int applicationId)
        {
            return _chatPrefix + applicationId;
        }

        private async Task SendMessageToGroup(int applicationId, MessageType type, string author, string text = null, int? blobId = null)
        {
            var messageDto = new PortalChatMessageListDto
            {
                Type = type,
                Text = text,
                Author = author,
                BlobId = blobId
            };

            await _hubContext.Clients.Group(GetGroupName(applicationId)).SendAsync("ReceiveMessage", messageDto);
        }
    }
}
