using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels
{
    [Table("SchoolLanguages")]
    public class SchoolLanguages
    {
        [Column] [PrimaryKey] public int SchoolId { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }

        [Column] public string Name { get; set; }
        [Column] public string Description { get; set; }
        [Column] public string Url { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string? Metadata { get; set; }
    }
}