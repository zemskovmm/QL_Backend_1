using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels
{
    [Table("CommonTraitsToHousingAccommodationType")]
    public class CommonTraitToHousingAccommodationType
    {
        [PrimaryKey] public int HousingAccommodationTypeId { get; set; }
        [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
