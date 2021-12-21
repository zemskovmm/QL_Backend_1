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
		[RemoteUiField("Lat")]
		public string Lat { get; set; }
        [RemoteUiField("Lng")]
		public string Lng { get; set; }
        [RemoteUiField("Address")]		
		public string Address { get; set; }
		
        public JObject? Metadata { get; set; }
        public HousingLanguageLocationAdminDto Location { get; set; }
    }
}
