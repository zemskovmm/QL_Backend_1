using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto
{
    public class AdminDtoResponse<T>
    {
        public JObject Definition { get; set; }
        public T Value { get; set; }
    }
}
