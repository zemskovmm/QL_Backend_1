using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels
{
    [Table("HousingAccommodationTypes")]
    public class HousingAccommodationType : BaseNamedModel
    {
        [Column] public string Square { get; set; }
        [Column] public string Residents { get; set; }
        [Column] public int Price { get; set; }
        [Column] public int HousingId { get; set; }
    }
}
