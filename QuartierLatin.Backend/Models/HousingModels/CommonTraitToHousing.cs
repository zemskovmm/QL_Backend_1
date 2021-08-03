using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.HousingModels
{
    [Table("CommonTraitsToHousing")]
    public class CommonTraitToHousing
    {
        [PrimaryKey] public int HousingId { get; set; }
        [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
