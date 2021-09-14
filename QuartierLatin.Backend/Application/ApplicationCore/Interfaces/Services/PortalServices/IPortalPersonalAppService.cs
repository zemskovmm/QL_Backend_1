using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices
{
    public interface IPortalPersonalAppService
    {
        Task<int> CreateApplicationAsync(ApplicationType? type, int? entityId,
            JObject applicationInfo, JObject entityTypeSpecificApplicationInfo, int userId);
        Task<bool> UpdateApplicationAsync(int id, ApplicationType? type, int? entityId, JObject applicationInfo, JObject entityTypeSpecificApplicationInfo);
        Task<PortalApplication> GetApplicationAsync(int id);
        Task<(int totalItems, List<PortalApplication> portalApplications)> GetApplicationCatalogAsync(ApplicationType? type, ApplicationStatus? status, int page, int pageSize);
    }
}
