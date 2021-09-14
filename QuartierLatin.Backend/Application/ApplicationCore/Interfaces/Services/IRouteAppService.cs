using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services
{
    public interface IRouteAppService
    {
        public Task<JObject> GetPageByUrlAsync(string lang, string url);

        public Task<JObject> GetPageByUrlAdminAsync(string url);
    }
}
