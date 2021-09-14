using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels
{
    [Table("SchoolGalleries")]
    public class SchoolGallery
    {
        [Column] [PrimaryKey] public int SchoolId { get; set; }
        [Column] [PrimaryKey] public int ImageId { get; set; }
    }
}
