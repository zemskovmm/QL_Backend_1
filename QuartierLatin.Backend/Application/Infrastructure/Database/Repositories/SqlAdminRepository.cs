using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories
{
    internal class SqlAdminRepository : IAdminRepository
    {
        private readonly AppDbContextManager _db;

        public SqlAdminRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public Admin GetById(int id)
        {
            return _db.Exec(db => db.Admins.First(x => x.Id == id));
        }

        public Admin FindByLogin(string login) => 
            _db.Exec(db => db.Admins.FirstOrDefault(x => x.Email == login));

        public int Create(string email, Guid identityId, string name, string passwordHash, bool confirmed = false)
        {
            return _db.Exec(d => d.InsertWithInt32Identity(new Admin()
            {
                Email = email,
                AzureIdentityId = identityId,
                Name = name,
                PasswordHash = passwordHash,
                Confirmed = confirmed,
            }));
        }

        public Admin FindByConfirmCode(string code) =>
            _db.Exec(db => db.Admins.FirstOrDefault(x => x.ConfirmationCode == code));

        public void Update(Admin user)
        {
            _db.Exec(d => d.Update(user));
        }

        public List<Admin> GetByIds(IEnumerable<int> ids)
        {
            return _db.Exec(d => d.Admins.Where(u => ids.Contains(u.Id)).ToList());
        }
    }
}