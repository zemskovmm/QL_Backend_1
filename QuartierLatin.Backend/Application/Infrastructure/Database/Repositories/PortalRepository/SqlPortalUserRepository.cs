using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.PortalRepository
{
    public class SqlPortalUserRepository : IPortalUserRepository
    {
        private readonly AppDbContextManager _db;

        public SqlPortalUserRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> RegisterAsync(string email, string passwordHash)
        {
            return await _db.ExecAsync(async db =>
            {
                if (await db.PortalUsers.AnyAsync(user => user.Email == email))
                    return 0;

                return await db.InsertWithInt32IdentityAsync(new PortalUser
                {
                    Email = email,
                    PasswordHash = passwordHash,
                    RegistrationDate = DateTime.Now
                });
            });
        }

        public async Task<PortalUser> GetPortalUserByIdAsync(int userId)
        {
            return await _db.ExecAsync(db => db.PortalUsers.FirstOrDefaultAsync(user => user.Id == userId));
        }

        public async Task<PortalUser> LoginAsync(string email)
        {
            return await _db.ExecAsync(db =>
                db.PortalUsers.FirstOrDefaultAsync(user => user.Email == email));
        }

        public async Task UpdatePortalUserInfo(int portalUserId, string? firstName, string? lastName, string? phone, JObject? personalInfo)
        {
            var portalUser = await GetPortalUserByIdAsync(portalUserId);

            if (portalUser is null)
                return;

            portalUser.FirstName = firstName;
            portalUser.LastName = lastName;
            portalUser.Phone = phone;
            portalUser.PersonalInfo = personalInfo?.ToString();

            await _db.ExecAsync(db => db.UpdateAsync(portalUser));
            return;
        }

        public async Task<(int totalItems, List<PortalUser> users)> GetPortalUserAdminListAsync(string? firstName, string? lastName, string? email, string? phone, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var users = db.PortalUsers.AsQueryable();

                if (firstName is not null)
                    users = users.Where(user => user.FirstName.StartsWith(firstName));

                if (lastName is not null)
                    users = users.Where(user => user.LastName.StartsWith(lastName));

                if (email is not null)
                    users = users.Where(user => user.Email.StartsWith(email));

                if (phone is not null)
                    users = users.Where(user => user.Phone.StartsWith(phone));

                var totalCount = await users.CountAsync();

                return (totalItems: totalCount, await users.Skip(skip).Take(take).ToListAsync());
            });
        }
    }
}
