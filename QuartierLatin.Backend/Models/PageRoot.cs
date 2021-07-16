using LinqToDB.Mapping;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Models
{
    [Table("PageRoots")]
    public class PageRoot : BaseModel
    {
        [Column] public PageType PageType { get; set; }
    }
}
