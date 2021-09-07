using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Models.Portal;
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

        public async Task<int> RegisterAsync(string firstName, string lastName, string phone, string email, string passwordHash)
        {
            return await _db.ExecAsync(async db =>
            {
                if (await db.PortalUsers.AnyAsync(user => user.Email == email))
                    return 0;

                return await db.InsertWithInt32IdentityAsync(new PortalUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    Email = email,
                    PasswordHash = passwordHash
                });
            });
        }

        public async Task<PortalUser> GetPortalUserByIdAsync(int userId)
        {
            return await _db.ExecAsync(db => db.PortalUsers.FirstOrDefaultAsync(user => user.Id == userId));
        }

        public async Task<PortalUser> LoginAsync(string email, string passwordHash)
        {
            return await _db.ExecAsync(db =>
                db.PortalUsers.FirstOrDefaultAsync(user => user.Email == email && user.PasswordHash == passwordHash));
        }
    }
}
