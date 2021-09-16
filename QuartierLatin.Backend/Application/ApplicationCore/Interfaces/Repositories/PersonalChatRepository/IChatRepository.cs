using QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PersonalChatRepository
{
    public interface IChatRepository
    {
        Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int portalUserId);
        Task<List<ChatMessages>> GetChatMessagesAdminAsync(int applicationId);
        Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, string text, MessageType type);
        Task<bool> SendChatMessageAdminAsync(int applicationId, string text, MessageType type);
        Task<List<(Chat chat, PortalUser user)>> GetChatsAsync();
    }
}
