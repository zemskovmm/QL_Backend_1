using LinqToDB.Mapping;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("CommonTraitTypesForEntites")]
    public class CommonTraitTypesForEntity
    {
        [Column] [PrimaryKey] public int CommonTraitId { get; set; }
        [Column] private EntityType EntityType { get; set; }
    }
}
