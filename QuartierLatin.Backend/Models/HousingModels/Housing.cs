using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.HousingModels
{
    [Table("Housings")]
    public class Housing : BaseModel
    {
        [Column] public int? Price { get; set; }
        [Column] public int? ImageId { get; set; }
    }
}
