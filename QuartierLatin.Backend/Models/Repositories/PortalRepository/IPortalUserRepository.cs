using System.Threading.Tasks;
using QuartierLatin.Backend.Models.Portal;

namespace QuartierLatin.Backend.Models.Repositories.PortalRepository
{
    public interface IPortalUserRepository
    {
        Task<int> RegisterAsync(string firstName, string lastName, string phone, string email, string passwordHash);
        Task<PortalUser> GetPortalUserByIdAsync(int userId);
        Task<PortalUser> LoginAsync(string email, string passwordHash);
    }
}
