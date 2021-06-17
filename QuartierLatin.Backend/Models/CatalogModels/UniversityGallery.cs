using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("UniversityGalleries")]
    public class UniversityGallery
    {
        [Column] [PrimaryKey] public int UniversityId { get; set; }
        [Column] [PrimaryKey] public int ImageId { get; set; }
    }
}
