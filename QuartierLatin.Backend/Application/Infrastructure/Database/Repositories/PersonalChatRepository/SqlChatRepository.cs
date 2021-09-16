using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PersonalChatRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.PersonalChatRepository
{
    public class SqlChatRepository : IChatRepository
    {
        private readonly AppDbContextManager _db;

        public SqlChatRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int portalUserId)
        {
            return await _db.ExecAsync(async db =>
            {
                var chat = await GetChatByApplicationAndPortalUserIdAsync(db, applicationId, portalUserId);

                if (chat is null)
                    return null;

                return await GetMessagesByChatIdAsync(db, chat.Id);
            });
        }

        public async Task<List<ChatMessages>> GetChatMessagesAdminAsync(int applicationId)
        {
            return await _db.ExecAsync(async db =>
            {
                var chat = await GetChatByApplicationAndPortalUserIdAsync(db, applicationId);

                if (chat is null)
                    return null;

                return await GetMessagesByChatIdAsync(db, chat.Id);
            });
        }

        public async Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, MessageType type, string text = null, int? blobId = null)
        {
            return await _db.ExecAsync(async db =>
            {
                var chat = await GetChatByApplicationAndPortalUserIdAsync(db, applicationId, portalUserId);

                if (chat is null)
                {
                    var application = db.PortalApplications.FirstOrDefaultAsync(application =>
                        application.Id == applicationId && application.UserId == portalUserId);

                    if (application is null)
                        return false;

                    var newChat = new Chat
                    {
                        ApplicationId = applicationId, 
                        PortalUserId = portalUserId
                    };

                    var chatId = await db.InsertWithInt32IdentityAsync(newChat);

                    await CreateMessageAsync(db, "User", chatId, type, text, blobId);
                }

                await CreateMessageAsync(db, "User", chat.Id, type, text, blobId);

                return true;
            });
        }

        public async Task<bool> SendChatMessageAdminAsync(int applicationId, MessageType type, string text = null, int? blobId = null)
        {
            return await _db.ExecAsync(async db =>
            {
                var chat = await GetChatByApplicationAndPortalUserIdAsync(db, applicationId);

                if (chat is null)
                    return false;

                await CreateMessageAsync(db, "User", chat.Id, type, text, blobId);

                return true;
            });
        }

        public async Task<List<(Chat chat, PortalUser user)>> GetChatsAsync()
        {
            return await _db.ExecAsync(async db =>
            {
                var query = await (from map in db.Chats
                    join user in db.PortalUsers on map.ApplicationId equals user.Id
                    select new {map, user}).ToListAsync();

                return query.Select(queryItem => (chat: queryItem.map, user: queryItem.user)).ToList(); ;
            });
        }

        private async Task<List<ChatMessages>> GetMessagesByChatIdAsync(AppDbContext db, int chatId)=> 
            await db.ChatMessages.Where(message => message.ChatId == chatId).ToListAsync();

        private async Task<Chat> GetChatByApplicationAndPortalUserIdAsync(AppDbContext db, int applicationId, int? portalUserId = null)
        {
            if(portalUserId.HasValue)
                return await db.Chats.FirstOrDefaultAsync(chat => chat.ApplicationId == applicationId && chat.ApplicationId == applicationId);

            return await db.Chats.FirstOrDefaultAsync(chat => chat.ApplicationId == applicationId);
        }

        private async Task CreateMessageAsync(AppDbContext db, string author, int chatId, MessageType type, string text = null, int? blobId = null)
        {
            var newMessage = new ChatMessages
            {
                Author = author,
                ChatId = chatId,
                Text = text,
                MessageType = type,
                BlobId = blobId
            };

            await db.InsertWithInt32IdentityAsync(newMessage);
        }
    }
}
