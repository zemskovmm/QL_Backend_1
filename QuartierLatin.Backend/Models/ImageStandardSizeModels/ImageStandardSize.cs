using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.ImageStandardSizeModels
{
    [Table("ImageStandardSizes")]
    public class ImageStandardSize : BaseModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
