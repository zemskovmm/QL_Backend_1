using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface IBlobRepository
    {
        Task<int> CreateBlobIdAsync(string fileType, string originalFileName, int? storageFolderId = null);
        Task DeleteBlobAsync(int id);

        Task EditBlobAsync(int id, string fileType);

        Task<Blob> GetBlobInfoAsync(int id);
    }
}