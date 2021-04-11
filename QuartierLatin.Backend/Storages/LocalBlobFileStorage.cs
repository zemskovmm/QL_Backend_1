using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Storages
{
    public class LocalBlobFileStorage : IBlobFileStorage
    {
        private readonly string _blobsPath;
        private ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public LocalBlobFileStorage(string blobsPath)
        {
            _blobsPath = blobsPath;
        }

        public async Task CreateBlobAsync(int id, Stream s, int? dimension = null)
        {
            if (CheckIfExist(id, dimension)) return;

            var key = "";

            if (dimension is null)
            {
                key = id.ToString();
            }
            else
            {
                key = id.ToString() + "_" + dimension.Value.ToString();
            }

            var myLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await myLock.WaitAsync();

            try
            {
                if (!CheckIfExist(id, dimension))
                {
                    var path = "";

                    if (dimension is null)
                        path = GetPath(id, true);
                    else
                        path = GetPath(id, true, dimension);

                    await using var f = File.Create(path);
                    await s.CopyToAsync(f);
                }
            }
            finally
            {
                myLock.Release();
            }
            return;
        }

        public Stream OpenBlob(int id, int? dimension = null)
        {
            var path = "";

            if (dimension is null)
                path = GetPath(id, false);
            else
                path = GetPath(id, false, dimension);

            if (dimension is null)
                return File.OpenRead(path);
            else
                return File.OpenRead(path);
        }

        public async Task DeleteBlob(int id, int? dimension = null)
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

        public bool CheckIfExist(int id, int? dimension = null)
        {
            var path = "";

            if (dimension is null)
                path = GetPath(id, false);
            else
                path = GetPath(id, false, dimension);

            return File.Exists(path);
        }

        private string GetPath(int id, bool create, int? dimension = null)
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