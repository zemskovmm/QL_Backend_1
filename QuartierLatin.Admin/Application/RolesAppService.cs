using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Admin.Dto;
using QuartierLatin.Admin.Models.Repositories;

namespace QuartierLatin.Admin.Application
{
    public interface IRolesAppService
    {
        public Task<List<UserRoleDto>> GetUserRolesByIds(List<int> ids);
    }

    public class RolesAppService : IRolesAppService
    {
        private readonly IRoleRepository _roleRepository;

        public RolesAppService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<UserRoleDto>> GetUserRolesByIds(List<int> ids)
        {
            return (await _roleRepository.UserRolesByIds(ids.ToArray())).Select(UserRoleDto.FromModel).ToList();
        }
    }
}