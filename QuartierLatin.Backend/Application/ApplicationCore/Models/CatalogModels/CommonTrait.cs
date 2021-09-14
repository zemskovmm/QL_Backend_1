using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels
{
    [Table("CommonTraits")]
    public class CommonTrait : BaseNamedModel
    {
        [PrimaryKey, Identity] public int Id { get; set; }
        [Column] public int CommonTraitTypeId { get; set; }
        [Column] public int? IconBlobId { get; set; } 
        [Column] public int Order { get; set; }
        [Column] public int? ParentId { get; set; }
        [Column] public string Identifier { get; set; }
    }
}
