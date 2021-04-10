using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class PageDto
    {
        public Dictionary<string, PageLanguageDto> Languages { get; set; }
    }
}
