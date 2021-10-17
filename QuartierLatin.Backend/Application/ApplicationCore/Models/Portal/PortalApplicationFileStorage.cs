using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.Portal
{
    [Table("PortalApplicationFileStorages")]
    public class PortalApplicationFileStorage
    {
        [Column] [PrimaryKey] public int ApplicationId { get; set; }
        [Column] [PrimaryKey] public int BlobId { get; set; }
    }
}
