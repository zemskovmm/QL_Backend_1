using LinqToDB.Mapping;

namespace QuartierLatin.Admin.Models
{
    [Table("Blobs")]
    public class Blob
    {
        [PrimaryKey] [Identity] public long Id { get; set; }

        // TODO: Storage type and stuff
    }
}