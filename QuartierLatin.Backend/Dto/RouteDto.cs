using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto
{
    public class RouteDto<T>
    {
        [JsonProperty("moduleName")]
        public string ModuleName { get; protected set; }
        [JsonProperty("urls")]
        public Dictionary<string, string> Urls { get; protected set; }
        [JsonProperty("module")]
        public T Module { get; protected set; }

        public RouteDto(string urlPrefix, Dictionary<string, string> urls, T module, string moduleName)
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
        }
    }
}
