using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.PortalRepository
{
    public class SqlPortalApplicationFileStorageRepository : IPortalApplicationFileStorageRepository
    {
        private readonly AppDbContextManager _db;
        public SqlPortalApplicationFileStorageRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<int>> GetFilesToApplicationAsync(int applicationId)
        {
            return await _db.ExecAsync(db =>
                db.PortalApplicationFileStorages.Where(file => file.ApplicationId == applicationId)
                    .Select(files => files.BlobId).ToListAsync());
        }

        public async Task CreateFileItemToApplicationAsync(int applicationId, int blobId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new PortalApplicationFileStorage
            {
                ApplicationId = applicationId,
                BlobId = blobId
            }));
        }

        public async Task DeleteFileItemToApplicationAsync(int applicationId, int blobId)
        {
            await _db.ExecAsync(db =>
                db.PortalApplicationFileStorages
                    .Where(file => file.ApplicationId == applicationId && file.BlobId == blobId)
                    .DeleteAsync());
        }
    }
}
