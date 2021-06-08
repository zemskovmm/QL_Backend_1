using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CurseCatalogModels.SchoolModels
{
    [Table("Schools")]
    public class School : BaseModel
    {
        [Column] public int? FoundationYear { get; set; }
    }
}