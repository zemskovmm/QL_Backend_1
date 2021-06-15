using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels
{
    [Table("CommonTraitToCourse")]
    public class CommonTraitToCourse
    {
        [Column] [PrimaryKey] public int CourseId { get; set; }
        [Column] [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
