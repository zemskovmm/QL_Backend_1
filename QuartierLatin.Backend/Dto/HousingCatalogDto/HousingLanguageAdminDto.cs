using Newtonsoft.Json.Linq;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto
{
    public class HousingLanguageAdminDto
    {
        [RemoteUiField("Name")]
        public string Name { get; set; }
        [RemoteUiField("HtmlDescription")]
        public string HtmlDescription { get; set; }
        [RemoteUiField("Url")]
        public string Url { get; set; }

		
        public JObject? Metadata { get; set; }
		
		[RemoteUiField("Location")]
        public HousingLanguageLocationAdminDto Location { get; set; }
    }
}
