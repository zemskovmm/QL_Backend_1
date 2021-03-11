using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    public class DataBlock : BaseModel
    {
        [Column] public string Title { get; set; }
        [Column] public string BlockData { get; set; }
        [Column] public int LanguageId { get; set; }
        [Column] public int PageId { get; set; }
    }
}
