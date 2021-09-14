using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels
{
    [Table("HousingGalleries")]
    public class HousingGallery
    {
        [Column] [PrimaryKey] public int HousingId { get; set; }
        [Column] [PrimaryKey] public int ImageId { get; set; }
    }
}
