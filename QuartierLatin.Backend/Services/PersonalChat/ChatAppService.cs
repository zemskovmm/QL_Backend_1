using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PersonalChatRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Services.PersonalChat
{
    public class ChatAppService : IChatAppService
    {
        private readonly IChatRepository _chatRepository;

        public ChatAppService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, MessageType type, string text = null, int? blobId = null)
        {
            var response = await _chatRepository.SendChatMessageAsync(applicationId, portalUserId, type, text, blobId);

            return response;
        }

        public async Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int count, int? beforeMessageId = null, int? afterMessageId = null)
        {
            return await _chatRepository.GetChatMessagesAsync(applicationId, count, beforeMessageId, afterMessageId);
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
