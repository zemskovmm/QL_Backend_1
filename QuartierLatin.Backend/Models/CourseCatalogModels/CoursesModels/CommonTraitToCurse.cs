using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels
{
    [Table("CommonTraitToCourse")]
    public class CommonTraitToCourse
    {
        [PrimaryKey] public int CourseId { get; set; }
        [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
