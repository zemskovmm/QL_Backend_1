using System.Collections.Generic;
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

        public RouteDto(Dictionary<string, string> urls, T module, string moduleName)
        {
            ModuleName = moduleName;
            Urls = urls;
            Module = module;
        }
    }
}
