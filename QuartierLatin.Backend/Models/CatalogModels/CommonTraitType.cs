using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("CommonTraitTypes")]
    public class CommonTraitType : BaseNamedModel
    {
        [PrimaryKey, Identity] public virtual int Id { get; set; }
        [Column] public string? Identifier { get; set; }
        [Column] public int Order { get; set; }
    }
}
