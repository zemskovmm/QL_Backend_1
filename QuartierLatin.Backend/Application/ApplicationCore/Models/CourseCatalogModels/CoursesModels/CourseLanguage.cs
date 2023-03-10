using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels
{
    [Table("CourseLanguages")]
    public class CourseLanguage
    {
        [Column] [PrimaryKey] public int CourseId { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }

        [Column] public string Name { get; set; }
        [Column] public string Description { get; set; }
        [Column] public string Url { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string? Metadata { get; set; }
    }
}