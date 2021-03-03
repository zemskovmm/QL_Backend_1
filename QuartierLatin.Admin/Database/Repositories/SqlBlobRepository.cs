using System.Linq;
using QuartierLatin.Admin.Models;
using QuartierLatin.Admin.Models.Repositories;
using LinqToDB;

namespace QuartierLatin.Admin.Database.Repositories
{
    public class SqlBlobRepository : IBlobRepository
    {
        private readonly AppDbContextManager _db;

        public SqlBlobRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public long CreateBlobId()
        {
            return _db.Exec(db => db.InsertWithInt64Identity(new Blob()));
        }

        public void DeleteBlob(long id) =>
            _db.Exec(db => db.Blobs.Select(x => x.Id == id).Delete());
    }
}