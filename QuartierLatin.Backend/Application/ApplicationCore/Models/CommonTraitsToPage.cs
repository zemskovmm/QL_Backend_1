using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    [Table("CommonTraitsToPages")]
    public class CommonTraitsToPage
    {
        [Column] [PrimaryKey] public int PageId { get; set; }
        [Column] [PrimaryKey] public int CommonTraitId { get; set; }
    }
}
