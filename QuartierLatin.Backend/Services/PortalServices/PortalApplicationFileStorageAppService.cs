using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Services.PortalServices
{
    public class PortalApplicationFileStorageAppService : IPortalApplicationFileStorageAppService
    {
        private readonly IPortalApplicationFileStorageRepository _portalApplicationFileStorageRepository;

        public PortalApplicationFileStorageAppService(IPortalApplicationFileStorageRepository portalApplicationFileStorageRepository)
        {
            _portalApplicationFileStorageRepository = portalApplicationFileStorageRepository;
        }

        public async Task<List<int>> GetFilesToApplicationAsync(int applicationId)
        {
            return await _portalApplicationFileStorageRepository.GetFilesToApplicationAsync(applicationId);
        }

        public async Task CreateFileItemToApplicationAsync(int applicationId, int blobId)
        {
            await _portalApplicationFileStorageRepository.CreateFileItemToApplicationAsync(applicationId, blobId);
        }

        public async Task DeleteFileItemToApplicationAsync(int applicationId, int blobId)
        {
            await _portalApplicationFileStorageRepository.DeleteFileItemToApplicationAsync(applicationId, blobId);
        }
    }
}
