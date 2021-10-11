using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.PortalRepository
{
    public class SqlPortalPersonalRepository : IPortalPersonalRepository
    {
        private readonly AppDbContextManager _db;

        public SqlPortalPersonalRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateApplicationAsync(ApplicationType? type, int? entityId, JObject applicationInfo,
            JObject entityTypeSpecificApplicationInfo, int userId)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new PortalApplication
            {
                UserId = userId,
                Status = ApplicationStatus.New,
                Type = type,
                CommonTypeSpecificApplicationInfo = applicationInfo.ToString(),
                EntityId = entityId,
                EntityTypeSpecificApplicationInfo = entityTypeSpecificApplicationInfo.ToString(),
                Date = DateTime.Now,
                IsAnswered = false,
                IsNewMessages = false
            }));
        }

        public async Task<bool> UpdateApplicationAsync(int id, int userid, ApplicationType? type, int? entityId, JObject applicationInfo,
            JObject entityTypeSpecificApplicationInfo)
        {
            return await _db.ExecAsync(async db =>
            {
                var applicationPortal = await db.PortalApplications.FirstOrDefaultAsync(portal => portal.Id == id && portal.UserId == userid);

                if (applicationPortal is null)
                    return false;

                if (applicationPortal.Status == ApplicationStatus.Review ||
                    applicationPortal.Status == ApplicationStatus.SentToEntity ||
                    applicationPortal.Status == ApplicationStatus.Fulfilled)
                    return false;

                await db.UpdateAsync(new PortalApplication
                {
                    Id = id,
                    Type = type,
                    Status = ApplicationStatus.Review,
                    UserId = applicationPortal.UserId,
                    CommonTypeSpecificApplicationInfo = applicationInfo is null ? null : applicationInfo.ToString(),
                    EntityId = entityId,
                    EntityTypeSpecificApplicationInfo = entityTypeSpecificApplicationInfo is null ? null : entityTypeSpecificApplicationInfo.ToString(),
                    Date = applicationPortal.Date,
                    IsAnswered = false,
                    IsNewMessages = applicationPortal.IsNewMessages
                });
                return true;
            });
        }

        public async Task<PortalApplication> GetApplicationAsync(int id, int userid)
        {
            return await _db.ExecAsync(db => db.PortalApplications.FirstOrDefaultAsync(portal => portal.Id == id && portal.UserId == userid));
        }

        public async Task<(int totalItems, List<PortalApplication> portalApplications)> GetApplicationCatalogAsync(int userid, ApplicationType? type, ApplicationStatus? status, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var portalApplications = db.PortalApplications.Where(portal => portal.UserId == userid).AsQueryable();

                if (type is not null)
                    portalApplications = portalApplications.Where(portal => portal.Type == type);

                if (status is not null)
                    portalApplications = portalApplications.Where(portal => portal.Status == status);

                var totalCount = await portalApplications.CountAsync();

                return (totalCount, await portalApplications.Skip(skip).Take(take).ToListAsync());
            });
        }

        public async Task<bool> CheckIsUserOwnerAsync(int userId, int applicationId)
        {
            return await _db.ExecAsync(async db =>
            {
                var portalApplication = await db.PortalApplications.FirstOrDefaultAsync(application =>
                    application.UserId == userId && application.Id == applicationId);

                return portalApplication is not null;
            });
        }

        public async Task<(int totalItems, List<(PortalApplication application, PortalUser user)> portalApplications)> GetApplicationCatalogAdminAsync(ApplicationType? type, ApplicationStatus? status, bool? isAnswered,
            string? firstName, string? lastName, string? email, string? phone, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var portalApplications = from application in db.PortalApplications
                    join user in db.PortalUsers on application.UserId equals user.Id
                    select new
                    {
                        application,
                        user
                    };

                if (type is not null)
                    portalApplications = portalApplications.Where(portal => portal.application.Type == type);

                if (status is not null)
                    portalApplications = portalApplications.Where(portal => portal.application.Status == status);

                if (isAnswered is not null)
                    portalApplications = portalApplications.Where(portal => portal.application.IsAnswered == isAnswered);

                if (firstName is not null)
                    portalApplications = portalApplications.Where(portal => portal.user.FirstName.StartsWith(firstName));

                if (lastName is not null)
                    portalApplications = portalApplications.Where(portal => portal.user.LastName.StartsWith(lastName));

                if (email is not null)
                    portalApplications = portalApplications.Where(portal => portal.user.Email.StartsWith(email));

                if (phone is not null)
                    portalApplications = portalApplications.Where(portal => portal.user.Phone.StartsWith(phone));

                var totalCount = await portalApplications.CountAsync();

                return (totalCount, portalApplications.Skip(skip).Take(take).ToList().Select(application => (application: application.application, user: application.user)).ToList());
            });
        }
    }
}
