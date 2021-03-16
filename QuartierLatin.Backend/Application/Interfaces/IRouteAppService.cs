using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IRouteAppService
    {
        public Task<JObject> GetPageByUrlAsync(string url);

        public Task<JObject> GetPageByUrlAdminAsync(string url);
    }
}
