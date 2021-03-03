using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Admin.Application;
using QuartierLatin.Admin.Auth.Requirements;
using QuartierLatin.Admin.Dto;
using QuartierLatin.Admin.Models;

namespace QuartierLatin.Admin.Rpc.AdminRpc
{
    public class RolesRpc : UserRpcBase
    {
        private readonly IRolesAppService _appService;

        public RolesRpc(IRolesAppService appService)
        {
            _appService = appService;
        }

        public List<string> GetMyRoles()
        {
            return UserRoles;
        }

        [RpcRole(Roles.Admin)]
        public async Task<List<UserRoleDto>> GetUserRolesByIds(List<int> ids)
        {
            return await _appService.GetUserRolesByIds(ids);
        }
    }
}