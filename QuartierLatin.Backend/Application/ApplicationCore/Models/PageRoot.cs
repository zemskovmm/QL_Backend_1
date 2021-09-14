using LinqToDB.Mapping;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    [Table("PageRoots")]
    public class PageRoot : BaseModel
    {
        [Column] public PageType PageType { get; set; }
    }
}
