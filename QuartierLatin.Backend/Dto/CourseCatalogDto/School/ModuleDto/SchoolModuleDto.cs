using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.RouteDto;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.School.ModuleDto
{
    public class SchoolModuleDto
    {
        public string Title { get; set; }

        public string DescriptionHtml { get; set; }

        public int? FoundationYear { get; set; }

        public NamedTraitsModuleDto Traits { get; set; }

        public JObject? Metadata { get; set; }
        public int? ImageId { get; set; }
    }
}
