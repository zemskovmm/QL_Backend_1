using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels
{
    [Table("Courses")]
    public class Course : BaseModel
    {
        [Column] public int SchoolId { get; set; }
        [Column] public int? ImageId { get; set; }
        [Column] public int Price { get; set; }
    }
}