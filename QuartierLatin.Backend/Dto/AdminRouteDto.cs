using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace QuartierLatin.Backend.Dto
{
    public class AdminRouteDto<T>
    {
        [JsonProperty("moduleName")]
        public string ModuleName { get; protected set; }
        [JsonProperty("urls")]
        public Dictionary<string, string> Urls { get; protected set; }
        [JsonProperty("module")]
        public T Module { get; protected set; }
        [JsonProperty("title")]
        public Dictionary<string, string> Title { get; protected set; }
        public AdminRouteDto(string urlPrefix, Dictionary<string, string> urls, T module, string moduleName, Dictionary<string, string> title)
        {
            ModuleName = moduleName;
            Urls = urls.ToDictionary(x => x.Key, x =>
            {
                var url = "/" + x.Key + "/";
                if (urlPrefix != null)
                    url += urlPrefix + "/";
                url += x.Value;
                return url;
            });
            Module = module;
            Title = title;
        }
    }
}
