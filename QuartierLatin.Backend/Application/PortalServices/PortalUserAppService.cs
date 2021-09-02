using QuartierLatin.Backend.Application.Interfaces.PortalServices;
using QuartierLatin.Backend.Models.Repositories.PortalRepository;

namespace QuartierLatin.Backend.Application.PortalServices
{
    public class PortalUserAppService : IPortalUserAppService
    {
        private readonly IPortalUserRepository _portalUserRepository;
        public PortalUserAppService(IPortalUserRepository portalUserRepository)
        {
            _portalUserRepository = portalUserRepository;
        }
    }
}
