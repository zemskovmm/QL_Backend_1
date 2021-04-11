using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("Universities")]
    public class University : BaseModel
    {
        [Column] public string Website { get; set; }
        [Column] public int? FoundationYear { get; set; }
        [Column] public int? MinimumAge { get; set; }
    }
}
