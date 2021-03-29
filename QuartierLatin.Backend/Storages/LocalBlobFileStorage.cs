using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Storages
{
    public class LocalBlobFileStorage : IBlobFileStorage
    {
        private readonly string _blobsPath;

        public LocalBlobFileStorage(string blobsPath)
        {
            _blobsPath = blobsPath;
        }

        public async Task CreateBlobAsync(long id, Stream s, int? dimension = null)
        {
            var path = "";

            if (dimension is null)
                path = GetPath(id, true);
            else
                path = GetPath(id, true, dimension);

            await using var f = File.Create(path);
            await s.CopyToAsync(f);
        }

        public Stream OpenBlob(long id, int? dimension = null)
        {
            if (dimension is null)
                return File.OpenRead(GetPath(id, false));
            else
                return File.OpenRead(GetPath(id, false, dimension));
        }

        public async Task DeleteBlob(long id, int? dimension = null)
        {
            var path = "";

            if (dimension is null)
                path = GetPath(id, false);
            else
                path = GetPath(id, false, dimension);

            if (!File.Exists(path)) return;
            File.Delete(path);
            await Task.Yield();
        }

        public bool CheckIfExist(long id, int? dimension = null)
        {
            var path = "";

            if (dimension is null)
                path = GetPath(id, false);
            else
                path = GetPath(id, false, dimension);

            return File.Exists(path);
        }

        private string GetPath(long id, bool create, int? dimension = null)
        {
            var s = id.ToString();
            if (s.Length == 0)
                return Path.Combine(_blobsPath, s + ".bin");

            var ds = s.Substring(0, s.Length - 1);
            var dir = Path.Combine(ds.Select(x => x.ToString()).Prepend(_blobsPath).ToArray());
            if (create)
                Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, s.Last() + ".bin");

            if (dimension is null)
                return path;
            else
                return Path.Combine(dir, s.Last() + "_" + dimension.ToString()  + ".bin");
        }
    }
}