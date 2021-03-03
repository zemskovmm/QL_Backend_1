using System;
using System.Collections.Generic;
using System.Linq;
using QuartierLatin.Admin.Models;
using QuartierLatin.Admin.Models.Repositories;
using LinqToDB;

namespace QuartierLatin.Admin.Database.Repositories
{
    internal class SqlUserRepository : IUserRepository
    {
        private readonly AppDbContextManager _db;

        public SqlUserRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public User GetById(int id)
        {
            return _db.Exec(db => db.Users.First(x => x.Id == id));
        }

        public User FindByLogin(string login) => 
            _db.Exec(db => db.Users.FirstOrDefault(x => x.Email == login));

        public int Create(string email, Guid identityId, string name, string passwordHash, bool confirmed = false)
        {
            return _db.Exec(d => d.InsertWithInt32Identity(new User
            {
                Email = email,
                AzureIdentityId = identityId,
                Name = name,
                PasswordHash = passwordHash,
                Confirmed = confirmed,
            }));
        }

        public User FindByConfirmCode(string code) =>
            _db.Exec(db => db.Users.FirstOrDefault(x => x.ConfirmationCode == code));

        public void Update(User user)
        {
            _db.Exec(d => d.Update(user));
        }

        public List<User> GetByIds(IEnumerable<int> ids)
        {
            return _db.Exec(d => d.Users.Where(u => ids.Contains(u.Id)).ToList());
        }
    }
}