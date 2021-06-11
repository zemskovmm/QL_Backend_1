using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels
{
    [Table("Curses")]
    public class Curse : BaseModel
    {
        [Column] public int SchoolId { get; set; }
    }
}