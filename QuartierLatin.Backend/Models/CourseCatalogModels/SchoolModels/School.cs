using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels
{
    [Table("Schools")]
    public class School : BaseModel
    {
        [Column] public int? FoundationYear { get; set; }
    }
}