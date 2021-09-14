using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.FolderModels
{
    [Table("StorageFolders")]
    public class StorageFolder : BaseModel
    {
        [Column] public string FolderName { get; set; }
        [Column] public int? FolderParentId { get; set; }
        [Column] public bool IsDeleted { get; set; }
    }
}
