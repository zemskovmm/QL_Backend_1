using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("CommonTraitsToUniversities")]
    public class CommonTraitsToUniversity
    {
        [Column] [PrimaryKey] public int UniversityId { get; set; }
        [Column] [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
