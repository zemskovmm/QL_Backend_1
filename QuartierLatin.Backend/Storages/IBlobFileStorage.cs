using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Storages
{
    public interface IBlobFileStorage
    {
        Task CreateBlobAsync(int id, Stream s, int? dimension = null, int? width = null, int? height = null);
        Stream OpenBlob(int id, int? dimension = null, int? width = null, int? height = null);
        Task DeleteBlob(int id, int? dimension = null, int? width = null, int? height = null);
        bool CheckIfExist(int id, int? dimension = null, int? width = null, int? height = null);
    }
}