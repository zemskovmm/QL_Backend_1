using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IBlobRepository
    {
        Task<long> CreateBlobIdAsync(string fileType, string originalFileName);
        Task DeleteBlobAsync(long id);

        Task EditBlobAsync(long id, string fileType);

        Task<Blob> GetBlobInfoAsync(long id);

        Task<Blob> GetBlobInfoByFileNameAndFileTypeAsync(string fileName, string fileType);
    }
}