using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels
{
    [Table("CommonTraitToCurse")]
    public class CommonTraitToCurse
    {
        [Column] [PrimaryKey] public int CurseId { get; set; }
        [Column] [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
