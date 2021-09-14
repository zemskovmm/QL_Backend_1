using System;
using System.Collections.Generic;
using QuartierLatin.Backend.Application.ApplicationCore.Models;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface IAdminRepository : IPasswordAuthRepository<Admin>
    {
        int Create(string email, Guid identityId, string name, string passwordHash, bool confirmed = false);
        void Update(Admin admin);
        Admin FindByConfirmCode(string code);
        List<Admin> GetByIds(IEnumerable<int> ids);
    }
}