using System.Threading.Tasks;
using QuartierLatin.Backend.Models.Portal;

namespace QuartierLatin.Backend.Application.Interfaces.PortalServices
{
    public interface IPortalUserAppService
    {
        Task<int> RegisterAsync(string firstName, string lastName, string phone, string email, string password);
        Task<PortalUser> GetPortalUserByIdAsync(int userId);
        Task<PortalUser> LoginAsync(string email, string password);
    }
}
