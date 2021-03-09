using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IRoleRepository
    {
        public Task AttachRole(UserRole role);
        public void AttachRoles(int userId, List<UserRole> roles);
        public Task RemoveRole(int id);
        public Task<List<UserRole>> UserRolesByIds(params int[] userId);
        public Task<bool> UserHasRole(int userId, string role);
    }
}