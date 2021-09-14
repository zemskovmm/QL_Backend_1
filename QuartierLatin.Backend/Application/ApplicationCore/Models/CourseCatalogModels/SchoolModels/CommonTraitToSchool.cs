using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels
{
    [Table("CommonTraitsToSchool")]
    public class CommonTraitToSchool
    {
        [PrimaryKey] public int SchoolId { get; set; }
        [PrimaryKey] public int CommonTraitId { get; set; }
    }
}