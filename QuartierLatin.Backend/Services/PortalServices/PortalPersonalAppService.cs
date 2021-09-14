using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Services.PortalServices
{
    public class PortalPersonalAppService : IPortalPersonalAppService
    {
        private readonly IPortalPersonalRepository _portalPersonalRepository;

        public PortalPersonalAppService(IPortalPersonalRepository portalPersonalRepository)
        {
            _portalPersonalRepository = portalPersonalRepository;
        }

        public async Task<int> CreateApplicationAsync(ApplicationType? type, int? entityId, JObject applicationInfo,
            JObject entityTypeSpecificApplicationInfo, int userId)
        {
            return await _portalPersonalRepository.CreateApplicationAsync(type, entityId, applicationInfo,
                entityTypeSpecificApplicationInfo, userId);
        }

        public async Task<bool> UpdateApplicationAsync(int id, ApplicationType? type, int? entityId, JObject applicationInfo,
            JObject entityTypeSpecificApplicationInfo)
        {
            return await _portalPersonalRepository.UpdateApplicationAsync(id, type, entityId, applicationInfo, entityTypeSpecificApplicationInfo);
        }

        public async Task<PortalApplication> GetApplicationAsync(int id)
        {
            return await _portalPersonalRepository.GetApplicationAsync(id);
        }

        public async Task<(int totalItems, List<PortalApplication> portalApplications)> GetApplicationCatalogAsync(ApplicationType? type, ApplicationStatus? status, int page, int pageSize)
        {
            return await _portalPersonalRepository.GetApplicationCatalogAsync(type, status, pageSize * page, pageSize);
        }
    }
}
