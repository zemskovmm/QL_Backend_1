using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.TraitTypeDto
{
    public class TraitTypeListDto
    {
        public int Id { get; set; }
        public JObject Names { get; set; }
        public string? Identifier { get; set; }
    }
}
