using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Dto;

namespace QuartierLatin.Backend.Services
{
    public interface IRolesAppService
    {
        public Task<List<AdminRoleDto>> GetUserRolesByIds(List<int> ids);
    }

    public class RolesAppService : IRolesAppService
    {
        private readonly IRoleRepository _roleRepository;

        public RolesAppService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<AdminRoleDto>> GetUserRolesByIds(List<int> ids)
        {
            return (await _roleRepository.UserRolesByIds(ids.ToArray())).Select(AdminRoleDto.FromModel).ToList();
        }
    }
}