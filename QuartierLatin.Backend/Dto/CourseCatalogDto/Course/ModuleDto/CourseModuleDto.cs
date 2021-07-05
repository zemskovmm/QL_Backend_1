﻿namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto
{
    public class CourseModuleDto
    {
        public string Title { get; set; }

        public string DescriptionHtml { get; set; }

        public int SchoolId { get; set; }

        public CourseModuleTraitsDto Traits { get; set; }
    }
}