using Newtonsoft.Json.Linq;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto
{
    public class HousingLanguageLocationAdminDto
    {

        [RemoteUiField("Lat")]
		public string Lat { get; set; }
        [RemoteUiField("Lng")]
		public string Lng { get; set; }
        [RemoteUiField("Address")]		
		public string Address { get; set; }

    }
}
