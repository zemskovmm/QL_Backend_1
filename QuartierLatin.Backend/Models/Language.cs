using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    public class Language : BaseModel
    {
        [Column] public string LanguageName { get; set; }
        [Column] public string LanguageShortName { get; set; }
    }
}
