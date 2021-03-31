using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.TraitTypeDto
{
    public class TraitTypeDto
    {
        public JObject Names { get; set; }
        public string? Identifier { get; set; }
    }
}
