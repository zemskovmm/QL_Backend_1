using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class PageDto
    {
        public Dictionary<string, PageLanguageDto> Languages { get; set; }
        public PageType PageType { get; set; }
    }
}
