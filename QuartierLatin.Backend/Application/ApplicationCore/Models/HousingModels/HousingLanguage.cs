using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels
{
    [Table("HousingLanguages")]
    public class HousingLanguage
    {
        [Column] [PrimaryKey] public int HousingId { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }

        [Column] public string Name { get; set; }
        [Column] public string Description { get; set; }
        [Column] public string Url { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string? Metadata { get; set; }
    }
}
