using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PersonalChatRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Services.PersonalChat
{
    public class ChatAppService : IChatAppService
    {
        private readonly IPortalUserAppService _portalUserAppService;
        private readonly IUserAppService _adminUserAppService;
        private readonly IChatRepository _chatRepository;
        private readonly IPortalPersonalAppService _portalPersonalAppService;
        private readonly string _chatPrefix = "personal-chat-";

        public ChatAppService(IPortalUserAppService portalUserAppService, IUserAppService userAppService, IChatRepository chatRepository, 
            IPortalPersonalAppService portalPersonalAppService)
        {
            _portalUserAppService = portalUserAppService;
            _adminUserAppService = userAppService;
            _chatRepository = chatRepository;
            _portalPersonalAppService = portalPersonalAppService;
        }

        public async Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int portalUserId)
        {
            return await _chatRepository.GetChatMessagesAsync(applicationId, portalUserId);
        }

        public async Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, MessageType type, string text = null, int? blobId = null)
        {
            var response = await _chatRepository.SendChatMessageAsync(applicationId, portalUserId, type, text, blobId);

            return response;
        }

        public async Task<List<ChatMessages>> GetChatMessagesAdminAsync(int applicationId)
        {
            return await _chatRepository.GetChatMessagesAdminAsync(applicationId);
        }

        public async Task<bool> SendChatMessageAdminAsync(int applicationId, MessageType type, string text = null, int? blobId = null)
        {
            var response = await _chatRepository.SendChatMessageAdminAsync(applicationId, type, text, blobId);

            return response;
        }

        public async Task<List<(Chat chat, PortalUser user)>> GetChatsAsync()
        {
            return await _chatRepository.GetChatsAsync();
        }
    }
}
