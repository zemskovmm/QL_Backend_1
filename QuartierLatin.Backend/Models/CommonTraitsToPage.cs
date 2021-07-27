using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    [Table("CreateCommonTraitsToPages")]
    public class CommonTraitsToPage
    {
        [Column] [PrimaryKey] public int PageId { get; set; }
        [Column] [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
