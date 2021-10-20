using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository
{
    public interface IPortalApplicationFileStorageRepository
    {
        Task<List<int>> GetFilesToApplicationAsync(int applicationId);
        Task CreateFileItemToApplicationAsync(int applicationId, int blobId);
        Task DeleteFileItemToApplicationAsync(int applicationId, int blobId);
    }
}
