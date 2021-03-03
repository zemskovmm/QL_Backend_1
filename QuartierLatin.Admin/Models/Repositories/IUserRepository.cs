using System;
using System.Collections.Generic;

namespace QuartierLatin.Admin.Models.Repositories
{
    public interface IUserRepository : IPasswordAuthRepository<User>
    {
        int Create(string email, Guid identityId, string name, string passwordHash);
        void Update(User user);
        User FindByConfirmCode(string code);
        List<User> GetByIds(IEnumerable<int> ids);
    }
}