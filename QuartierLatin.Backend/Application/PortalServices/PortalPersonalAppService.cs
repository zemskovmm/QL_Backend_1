using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.PortalServices;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Portal;
using QuartierLatin.Backend.Models.Repositories.PortalRepository;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.PortalServices
{
    public class PersonalAppService : IPersonalAppService
    {
        private readonly IPortalPersonalRepository _portalPersonalRepository;

        public PersonalAppService(IPortalPersonalRepository portalPersonalRepository)
        {
            _portalPersonalRepository = portalPersonalRepository;
        }

        public async Task<int> CreateApplicationAsync(ApplicationType type, int entityId, JObject applicationInfo,
            JObject entityTypeSpecificApplicationInfo)
        {
            return await _portalPersonalRepository.CreateApplicationAsync(type, entityId, applicationInfo,
                entityTypeSpecificApplicationInfo);
        }

        public async Task UpdateApplicationAsync(int id, ApplicationType type, int entityId, JObject? applicationInfo,
            JObject? entityTypeSpecificApplicationInfo)
        {
            await _portalPersonalRepository.UpdateApplicationAsync(id, type, entityId, applicationInfo, entityTypeSpecificApplicationInfo);
        }

        public async Task<PortalApplication> GetApplicationAsync(int id)
        {
            return await _portalPersonalRepository.GetApplicationAsync(id);
        }
    }
}
