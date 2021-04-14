using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("UniversityInstructionLanguages")]
    public class UniversityInstructionLanguage
    {
        [Column] [PrimaryKey] public int UniversityId { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }
    }
}