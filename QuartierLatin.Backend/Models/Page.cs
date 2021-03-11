using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    public class Page : BaseModel
    {
        [Column] public string Url { get; set; }
        [Column] public int LanguageId { get; set; }
        [Column] public int PageRootId { get; set; }
    }
}
