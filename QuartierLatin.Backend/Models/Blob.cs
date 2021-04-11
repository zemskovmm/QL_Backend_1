using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    [Table("Blobs")]
    public class Blob
    {
        [PrimaryKey, Identity] public int Id { get; set; }

        [Column] public string FileType { get; set; }

        [Column] public string OriginalFileName { get; set; }
    }
}