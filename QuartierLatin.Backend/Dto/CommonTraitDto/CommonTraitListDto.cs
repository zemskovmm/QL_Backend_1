using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.CommonTraitDto
{
    public class CommonTraitListDto
    {
        public int Id { get; set; }
        public int CommonTraitTypeId { get; set; }
        public JObject Names { get; set; }
        public long? IconBlobId { get; set; }
        public int Order { get; set; }
    }
}
