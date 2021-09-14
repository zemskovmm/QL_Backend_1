using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository
{
    public interface IPortalUserRepository
    {
        Task<int> RegisterAsync(string firstName, string lastName, string phone, string email, string passwordHash, JObject personalInfo);
        Task<PortalUser> GetPortalUserByIdAsync(int userId);
        Task<PortalUser> LoginAsync(string email);
    }
}
