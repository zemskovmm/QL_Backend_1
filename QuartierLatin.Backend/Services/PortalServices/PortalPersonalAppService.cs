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
            JObject entityTypeSpecificApplicationInfo, bool? isActive = null)
        {
            return await _portalPersonalRepository.UpdateApplicationAsync(id, type, entityId, applicationInfo, entityTypeSpecificApplicationInfo, isActive);
        }

        public async Task<PortalApplication> GetApplicationAsync(int id)
        {
            return await _portalPersonalRepository.GetApplicationAsync(id);
        }

        public async Task<(int totalItems, List<PortalApplication> portalApplications)> GetApplicationCatalogAsync(int userid, ApplicationType? type, ApplicationStatus? status, int page, int pageSize)
        {
            return await _portalPersonalRepository.GetApplicationCatalogAsync(userid, type, status, pageSize * page, pageSize);
        }

        public async Task<bool> CheckIsUserOwnerAsync(int userId, int applicationId)
        {
            return await _portalPersonalRepository.CheckIsUserOwnerAsync(userId, applicationId);
        }

        public async Task<(int totalItems, List<(PortalApplication application, PortalUser user)> portalApplications)> GetApplicationCatalogAdminAsync(ApplicationType? type, ApplicationStatus? status, bool? isAnswered,
            string? firstName, string? lastName, string? email, string? phone, int? userId, int page, int pageSize)
        {
            return await _portalPersonalRepository.GetApplicationCatalogAdminAsync(type, status, isAnswered, firstName, lastName, email, phone, userId, pageSize * page, pageSize);
        }
    }
}
