using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CurseCatalogDto.Curse
{
    public class CurseAdminDto
    {
        public int SchoolId { get; set; }
        public Dictionary<string, CurseLanguageAdminDto> Languages { get; set; }
    }
}
