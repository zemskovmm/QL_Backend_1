using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Storages
{
    public interface IBlobFileStorage
    {
        Task CreateBlobAsync(long id, Stream s, int? dimension = null);
        Stream OpenBlob(long id, int? dimension = null);
        Task DeleteBlob(long id, int? dimension = null);

        bool CheckIfExist(long id, int? dimension = null);
    }
}