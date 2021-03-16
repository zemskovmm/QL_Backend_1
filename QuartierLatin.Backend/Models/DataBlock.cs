using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    [Table("DataBlocks")]
    public class DataBlock : BaseModel
    {
        [Column] public string Type { get; set; }
        [Column] public string BlockData { get; set; }
        [Column] public int LanguageId { get; set; }
        [Column] public int PageId { get; set; }
        [Column] public int BlockRootId { get; set; }
    }
}
