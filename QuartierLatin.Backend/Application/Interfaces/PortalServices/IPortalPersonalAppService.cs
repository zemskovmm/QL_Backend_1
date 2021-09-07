using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Portal;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.PortalServices
{
    public interface IPortalPersonalAppService
    {
        Task<int> CreateApplicationAsync(ApplicationType? type, int? entityId,
            JObject? applicationInfo, JObject? entityTypeSpecificApplicationInfo, int userId);
        Task<bool> UpdateApplicationAsync(int id, ApplicationType? type, int? entityId, JObject? applicationInfo, JObject? entityTypeSpecificApplicationInfo);
        Task<PortalApplication> GetApplicationAsync(int id);
        Task<(int totalItems, List<PortalApplication> portalApplications)> GetApplicationCatalogAsync(ApplicationType? type, ApplicationStatus? status, int page, int pageSize);
    }
}
