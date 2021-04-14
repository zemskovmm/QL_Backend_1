using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces;

namespace QuartierLatin.Backend.Application
{
    public class RouteAppService : IRouteAppService
    {
        private readonly IPageAppService _pageAppService;

        public RouteAppService(IPageAppService pageAppService)
        {
            _pageAppService = pageAppService;
        }

        public async Task<JObject> GetPageByUrlAdminAsync(string url)
        {
            var route = await _pageAppService.GetPageByUrlAdminAsync(url);

            var response = JObject.FromObject(route);

            return response;
        }

        public async Task<JObject> GetPageByUrlAsync(string lang, string url)
        {
            var route = await _pageAppService.GetPagesByRootIdAsync(lang, url);

            var response = JObject.FromObject(route);

            return response;
        }
    }
}
