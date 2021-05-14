using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.FolderModels
{
    [Table("StorageFolders")]
    public class StorageFolder : BaseModel
    {
        [Column] public string FolderName { get; set; }
        [Column] public int? FolderParentId { get; set; }
    }
}
