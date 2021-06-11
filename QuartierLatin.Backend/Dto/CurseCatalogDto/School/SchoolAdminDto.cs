﻿using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CurseCatalogDto.School
{
    public class SchoolAdminDto
    {
        public int? FoundationYear { get; set; }
        public Dictionary<string, SchoolLanguageAdminDto> Languages { get; set; }
    }
}