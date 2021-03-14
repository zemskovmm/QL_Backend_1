using System;
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

        public async Task<JObject> GetPageByUrl(string url)
        {
            var route = await _pageAppService.GetPageByUrl(url);

            var response = JObject.FromObject(route);

            return response;
        }
    }
}
