using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        public Task AttachRole(AdminRole role);
        public void AttachRoles(int userId, List<AdminRole> roles);
        public Task RemoveRole(int id);
        public Task<List<AdminRole>> UserRolesByIds(params int[] userId);
        public Task<bool> UserHasRole(int userId, string role);
    }
}