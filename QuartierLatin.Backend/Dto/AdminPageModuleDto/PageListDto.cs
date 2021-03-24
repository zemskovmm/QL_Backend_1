using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class PageListDto
    {
        public int TotalPages { get; set; }
        public List<PageListItemDto> Results { get; set; }
    }
}
