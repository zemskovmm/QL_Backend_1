using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    [Table("Languages")]
    public class Language : BaseModel
    {
        [Column] public string LanguageName { get; set; }
        [Column] public string LanguageShortName { get; set; }
    }
}
