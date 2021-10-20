using QuartierLatin.Backend.Dto.RouteDto;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto
{
    public class CourseListModuleDto
    {
        public string Url { get; set; }
        public string LanglessUrl { get; set; }
        public string Name { get; set; }
        public int? ImageId { get; set; }
        public int Price { get; set; }
        public string SchoolName { get; set; }
        public NamedTraitsModuleDto Traits { get; set; }
    }
}
