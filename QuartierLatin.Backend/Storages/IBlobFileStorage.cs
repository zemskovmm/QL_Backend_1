using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Storages
{
    public interface IBlobFileStorage
    {
        Task CreateBlobAsync(int id, Stream s, int? dimension = null);
        Stream OpenBlob(int id, int? dimension = null);
        Task DeleteBlob(int id, int? dimension = null);

        bool CheckIfExist(int id, int? dimension = null);
    }
}