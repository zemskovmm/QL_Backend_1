﻿namespace QuartierLatin.Backend.Dto.courseCatalogDto.School.ModuleDto
{
    public class SchoolModuleDto
    {
        public string Title { get; set; }

        public string DescriptionHtml { get; set; }

        public int? FoundationYear { get; set; }

        public SchoolModuleTraitsDto Traits { get; set; }
    }
}