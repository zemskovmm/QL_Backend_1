using QuartierLatin.Backend.Models.CallRequest;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface ICallRequestAppService
    {
        Task<bool> SendCallRequest(CallRequest requestCall);
    }
}
