using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CurseCatalogDto.Curse
{
    public class CurseListAdminDto : BaseDto
    {
        public int SchoolId { get; set; }
        public Dictionary<string, CurseLanguageAdminDto> Languages { get; set; }
    }
}
