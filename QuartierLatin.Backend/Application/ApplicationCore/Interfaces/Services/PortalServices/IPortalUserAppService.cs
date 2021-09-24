using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices
{
    public interface IPortalUserAppService
    {
        Task<int> RegisterAsync(string email, string password);
        Task<PortalUser> GetPortalUserByIdAsync(int userId);
        Task<PortalUser> LoginAsync(string email, string password);
        Task UpdateUserInfoAsync(int portalUserId, string? firstName, string? lastName, string? phone, JObject? personalInfo);
    }
}
