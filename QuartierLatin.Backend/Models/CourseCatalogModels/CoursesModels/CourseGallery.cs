using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels
{
    [Table("CourseGalleries")]
    public class CourseGallery
    {
        [Column] [PrimaryKey] public int CourseId { get; set; }
        [Column] [PrimaryKey] public int ImageId { get; set; }
    }
}
