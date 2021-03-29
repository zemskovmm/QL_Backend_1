using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("CommonTraitTypes")]
    public class CommonTraitType : BaseModel
    {
        [Column(DataType = DataType.BinaryJson)] public string Names { get; set; }
        [Column] public string? Identifier { get; set; }
    }
}
