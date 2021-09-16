using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat
{
    public interface IChatAppService
    {
        Task JoinUserToChatAsync(int userId, int applicationId, string connectionId);
        Task JoinAdminToChatAsync(int userId, int applicationId, string connectionId);
        Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int portalUserId);
        Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, MessageType type, string text = null, int? blobId = null);
        Task<List<ChatMessages>> GetChatMessagesAdminAsync(int applicationId);
        Task<bool> SendChatMessageAdminAsync(int applicationId, MessageType type, string text = null, int? blobId = null);
        Task<List<(Chat chat, PortalUser user)>> GetChatsAsync();
    }
}
