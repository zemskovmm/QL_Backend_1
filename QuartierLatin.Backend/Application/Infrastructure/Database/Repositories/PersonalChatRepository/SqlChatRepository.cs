using System;
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

        public async Task<List<ChatMessages>> GetChatMessagesAsync(int applicationId, int count, int? beforeMessageId = null, int? afterMessageId = null)
        {
            return await _db.ExecAsync(async db =>
            {
                var chat = await GetChatByApplicationAndPortalUserIdAsync(applicationId);

                if (chat is null)
                    return null;

                var messages = db.ChatMessages.Where(message => message.ChatId == chat.Id).AsQueryable();

                if (beforeMessageId is not null)
                    messages = messages.Where(message => message.Id <= beforeMessageId);

                if (afterMessageId is not null)
                    messages = messages.Where(message => message.Id >= afterMessageId);

                messages = messages.OrderByDescending(message => message.Id).Take(count);

                return await messages.OrderBy(message => message.Id).ToListAsync();
            });
        }

        public async Task<bool> SendChatMessageAsync(int applicationId, int portalUserId, MessageType type, string text = null, int? blobId = null)
        {
            return await _db.ExecAsync(async db =>
            {
                var chat = await GetChatByApplicationAndPortalUserIdAsync(applicationId, portalUserId);

                var application = await db.PortalApplications.FirstOrDefaultAsync(application =>
                    application.Id == applicationId);

                if (application is null)
                    return false;

                if (chat is null)
                {
                    var chatId = await db.InsertWithInt32IdentityAsync( new Chat
                    {
                        ApplicationId = application.Id,
                        PortalUserId = portalUserId
                    });

                    await CreateMessageAsync(db, "User", chatId, type, applicationId, text, blobId);
                }
                else
                {
                    await CreateMessageAsync(db, "User", chat.Id, type, applicationId, text, blobId);
                }

                application.IsNewMessages = true;
                application.IsAnswered = false;

                await db.UpdateAsync(application);

                return true;
            });
        }

        public async Task<bool> SendChatMessageAdminAsync(int applicationId, MessageType type, string text = null, int? blobId = null)
        {
            return await _db.ExecAsync(async db =>
            {
                var chat = await GetChatByApplicationAndPortalUserIdAsync(applicationId);

                if (chat is null)
                    return false;

                await CreateMessageAsync(db, "Manager", chat.Id, type, applicationId, text, blobId);

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

                return query.Select(queryItem => (chat: queryItem.map, user: queryItem.user)).ToList();
            });
        }

        private async Task<Chat> GetChatByApplicationAndPortalUserIdAsync(int applicationId, int? portalUserId = null)
        {
            return await _db.ExecAsync(async db =>
            {
                if (portalUserId.HasValue)
                    return await db.Chats.FirstOrDefaultAsync(chat => chat.ApplicationId == applicationId && chat.ApplicationId == applicationId);

                return await db.Chats.FirstOrDefaultAsync(chat => chat.ApplicationId == applicationId);
            });
        }

        private async Task CreateMessageAsync(AppDbContext db, string author, int chatId, MessageType type, int applicationId, string text = null, int? blobId = null)
        {
            var application = await db.PortalApplications.FirstOrDefaultAsync(application =>
                application.Id == applicationId);

            await _db.ExecAsync(async db =>
            {
                await db.BeginTransactionAsync();

                try
                {
                    var newMessage = new ChatMessages
                    {
                        Author = author,
                        ChatId = chatId,
                        Text = text,
                        MessageType = type,
                        BlobId = blobId,
                        Date = DateTime.Now
                    };

                    await db.InsertWithInt32IdentityAsync(newMessage);

                    application.IsAnswered = false;
                    application.IsNewMessages = true;
                    await db.UpdateAsync(application);
                }
                catch
                {
                    await db.RollbackTransactionAsync();
                }

                await db.CommitTransactionAsync();
            });
        }
    }
}
