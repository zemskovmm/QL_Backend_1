using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Admin.Storages
{
    public interface IBlobFileStorage
    {
        Task CreateBlobAsync(long id, Stream s);
        Stream OpenBlob(long id);
        Task DeleteBlob(long id);
    }
}