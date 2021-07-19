using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("UniversityLanguages")]
    public class UniversityLanguage
    {
        [Column] [PrimaryKey] public int UniversityId { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }

        [Column] public string Name { get; set; }
        [Column] public string Description { get; set; }
        [Column] public string Url { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string? Metadata { get; set; }
    }
}
