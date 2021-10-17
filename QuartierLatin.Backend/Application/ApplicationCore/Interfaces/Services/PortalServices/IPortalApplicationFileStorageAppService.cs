using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices
{
    public interface IPortalApplicationFileStorageAppService
    {
        Task<List<int>> GetFilesToApplicationAsync(int applicationId);
        Task CreateFileItemToApplicationAsync(int applicationId, int blobId);
        Task DeleteFileItemToApplicationAsync(int applicationId, int blobId);
    }
}
