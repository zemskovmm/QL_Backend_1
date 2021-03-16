using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class CreateDataBlockDto
    {
        public string Type { get; set; }
        public JObject BlockData { get; set; }
        public int LanguageId { get; set; }
        public int PageId { get; set; }
        public int BlockRootId { get; set; }
    }
}
