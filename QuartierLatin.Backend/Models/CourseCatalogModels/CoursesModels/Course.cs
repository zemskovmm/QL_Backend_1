using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels
{
    [Table("Courses")]
    public class Course : BaseModel
    {
        [Column] public int SchoolId { get; set; }
    }
}