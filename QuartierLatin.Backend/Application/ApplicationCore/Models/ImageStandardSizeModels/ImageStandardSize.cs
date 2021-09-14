using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.ImageStandardSizeModels
{
    [Table("ImageStandardSizes")]
    public class ImageStandardSize : BaseModel
    {
        [Column] public int Width { get; set; }
        [Column] public int Height { get; set; }
    }
}
