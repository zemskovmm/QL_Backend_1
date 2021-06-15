using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels
{
    [Table("CommonTraitsToSchool")]
    public class CommonTraitToSchool
    {
        [Column] [PrimaryKey] public int SchoolId { get; set; }
        [Column] [PrimaryKey] public int CommonTraitId { get; set; }
    }
}