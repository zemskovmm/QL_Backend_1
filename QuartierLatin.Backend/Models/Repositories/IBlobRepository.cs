using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IBlobRepository
    {
        Task<int> CreateBlobIdAsync(string fileType, string originalFileName);
        Task DeleteBlobAsync(int id);

        Task EditBlobAsync(int id, string fileType);

        Task<Blob> GetBlobInfoAsync(int id);
    }
}