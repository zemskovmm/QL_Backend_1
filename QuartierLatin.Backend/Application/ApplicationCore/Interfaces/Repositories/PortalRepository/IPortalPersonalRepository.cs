using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository
{
    public interface IPortalPersonalRepository
    {
        Task<int> CreateApplicationAsync(ApplicationType? type, int? entityId, JObject applicationInfo, JObject entityTypeSpecificApplicationInfo, int userId);
        Task<bool> UpdateApplicationAsync(int id, int userid, ApplicationType? type, int? entityId, JObject applicationInfo, JObject entityTypeSpecificApplicationInfo);
        Task<PortalApplication> GetApplicationAsync(int id, int userid);
        Task<(int totalItems, List<PortalApplication> portalApplications)> GetApplicationCatalogAsync(int userid, ApplicationType? type, ApplicationStatus? status, int skip, int take);
        Task<bool> CheckIsUserOwnerAsync(int userId, int applicationId);
        Task<(int totalItems, List<(PortalApplication application, PortalUser user)> portalApplications)> GetApplicationCatalogAdminAsync(ApplicationType? type, ApplicationStatus? status, bool? isAnswered,
            string? firstName, string? lastName, string? email, string? phone, int skip, int take);
    }
}
