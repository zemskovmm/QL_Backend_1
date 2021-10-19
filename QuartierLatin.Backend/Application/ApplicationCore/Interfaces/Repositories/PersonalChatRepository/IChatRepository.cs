using QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PersonalChatRepository
{
    public interface IChatRepository
    {
        Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int count, int? beforeMessageId = null, int? afterMessageId = null);
        Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, MessageType type, string text = null, int? blobId = null);
        Task<bool> SendChatMessageAdminAsync(int applicationId, MessageType type, string text = null, int? blobId = null);
        Task<List<(Chat chat, PortalUser user)>> GetChatsAsync();
    }
}
