using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CallRequest;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services
{
    public interface ICallRequestAppService
    {
        Task<bool> SendCallRequest(CallRequest requestCall);
    }
}
