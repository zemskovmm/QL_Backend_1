using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Portal;

namespace QuartierLatin.Backend.Models.Repositories.PortalRepository
{
    public interface IPortalUserRepository
    {
        Task<int> RegisterAsync(string firstName, string lastName, string phone, string email, string passwordHash, JObject personalInfo);
        Task<PortalUser> GetPortalUserByIdAsync(int userId);
        Task<PortalUser> LoginAsync(string email);
    }
}
