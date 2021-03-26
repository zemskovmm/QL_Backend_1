using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    [Table("Blobs")]
    public class Blob
    {
        [PrimaryKey, Identity] public long Id { get; set; }

        [Column] public string FileType { get; set; }

        [Column] public string OriginalFileName { get; set; }
    }
}