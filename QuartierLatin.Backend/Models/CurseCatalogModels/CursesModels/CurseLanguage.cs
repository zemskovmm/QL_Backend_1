using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels
{
    [Table("CurseLanguages")]
    public class CurseLanguage
    {
        [Column] [PrimaryKey] public int CurseId { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }

        [Column] public string Name { get; set; }
        [Column] public string Description { get; set; }
        [Column] public string Url { get; set; }
    }
}