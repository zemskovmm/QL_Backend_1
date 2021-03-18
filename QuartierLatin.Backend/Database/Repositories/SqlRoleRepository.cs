using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using LinqToDB;
using Npgsql;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlRoleRepository : IRoleRepository
    {
        private readonly AppDbContextManager _db;

        public SqlRoleRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<AdminRole>> UserRolesByIds(params int[] adminIds)
        {
            return _db.Exec(db => db.AdminRoles.Where(x => adminIds.Contains(x.AdminId)).ToList());
        }

        public async Task<bool> UserHasRole(int adminId, string role)
        {
            return _db.Exec(db => db.AdminRoles.Where(x => x.AdminId == adminId).Select(x => x.Role).Contains(role));
        }

        public async Task AttachRole(AdminRole role)
        {
            _db.Exec(db => db.Insert(role));
        }

        public void AttachRoles(int adminId, List<AdminRole> roles)
        {
            _db.Exec(db =>
            {
                using var trx = (db.Connection as NpgsqlConnection).BeginTransaction();
                db.AdminRoles.Where(x => x.AdminId == adminId).Delete();
                roles.ForEach(x => db.Insert(x));
                trx.Commit();
            });
        }

        public async Task RemoveRole(int id)
        {
            _db.Exec(db => db.AdminRoles.Where(x => x.Id == id).Delete());
        }

        public async Task AttachRole(int adminId, string role)
        {
            _db.Exec(db =>
            {
                var admin = db.Admins.FirstOrDefault(x => x.Id == adminId);
                if (admin == null) return;

                db.Insert(new AdminRole()
                {
                    Role = role,
                    AdminId = adminId
                });
            });
        }
    }
}