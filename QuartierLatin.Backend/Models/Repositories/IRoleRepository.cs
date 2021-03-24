using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
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