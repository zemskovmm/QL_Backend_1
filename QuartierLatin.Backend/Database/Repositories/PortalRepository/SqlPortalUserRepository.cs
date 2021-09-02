using QuartierLatin.Backend.Models.Repositories.PortalRepository;

namespace QuartierLatin.Backend.Database.Repositories.PortalRepository
{
    public class SqlPortalUserRepository : IPortalUserRepository
    {
        private readonly AppDbContextManager _db;

        public SqlPortalUserRepository(AppDbContextManager db)
        {
            _db = db;
        }
    }
}
