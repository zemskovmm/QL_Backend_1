using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto
{
    public class RouteDto<T>
    {
        public string ModuleName { get; protected set; } 
        public Dictionary<string, string> Urls { get; protected set; }
        public T Module { get; protected set; }

        public RouteDto(Dictionary<string, string> urls, T module, string moduleName)
        {
            ModuleName = moduleName;
            Urls = urls;
            Module = module;
        }
    }
}
