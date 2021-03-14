using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IRouteAppService
    {
        public Task<JObject> GetPageByUrl(string url);
    }
}
