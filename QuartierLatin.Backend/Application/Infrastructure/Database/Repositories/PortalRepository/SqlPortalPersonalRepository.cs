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
                EntityTypeSpecificApplicationInfo = entityTypeSpecificApplicationInfo.ToString()
            }));
        }

        public async Task<bool> UpdateApplicationAsync(int id, ApplicationType? type, int? entityId, JObject applicationInfo,
            JObject entityTypeSpecificApplicationInfo)
        {
            return await _db.ExecAsync(async db =>
            {
                var applicationPortal = await db.PortalApplications.FirstOrDefaultAsync(portal => portal.Id == id);

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
                    EntityTypeSpecificApplicationInfo = entityTypeSpecificApplicationInfo is null ? null : entityTypeSpecificApplicationInfo.ToString()
                });
                return true;
            });
        }

        public async Task<PortalApplication> GetApplicationAsync(int id)
        {
            return await _db.ExecAsync(db => db.PortalApplications.FirstOrDefaultAsync(portal => portal.Id == id));
        }

        public async Task<(int totalItems, List<PortalApplication> portalApplications)> GetApplicationCatalogAsync(ApplicationType? type, ApplicationStatus? status, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var portalApplications = db.PortalApplications.AsQueryable();

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
    }
}
