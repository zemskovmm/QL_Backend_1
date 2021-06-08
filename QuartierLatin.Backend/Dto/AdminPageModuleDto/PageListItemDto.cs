using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class PageListItemDto
    {
        public int Id { get; set; }
        public Dictionary<string, string> Urls { get; set; }
        public Dictionary<string, string> Titles { get; set; }
        public Dictionary<string, int?> PreviewImages { get; set; }
    }
}
