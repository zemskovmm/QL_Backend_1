using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.RouteDto;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto
{
    public class CourseModuleDto
    {
        public string Title { get; set; }

        public string DescriptionHtml { get; set; }

        public int SchoolId { get; set; }

        public NamedTraitsModuleDto Traits { get; set; }
        public JObject? Metadata { get; set; }
        public int? ImageId { get; set; }
    }
}
