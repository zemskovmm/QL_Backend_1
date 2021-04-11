using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("CommonTraits")]
    public class CommonTrait : BaseModel
    {
        [Column] public int CommonTraitTypeId { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string Names { get; set; }
        [Column] public int? IconBlobId { get; set; } 
        [Column] public int Order { get; set; }
        [Column] public int? ParentId { get; set; }
        [Column] public string Identifier { get; set; }
    }
}
