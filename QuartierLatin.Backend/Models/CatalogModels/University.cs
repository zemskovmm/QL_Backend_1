using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("Universities")]
    public class University : BaseModel
    {
        [Column] public int? FoundationYear { get; set; }
        [Column] public int? LogoId { get; set; }
        [Column] public int? BannerId { get; set; }
    }
}
