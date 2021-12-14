using LinqToDB.Mapping;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels
{
    [Table("CommonTraitTypesForEntites")]
    public class CommonTraitTypesForEntity
    {
        [Column] public int CommonTraitId { get; set; }
        [Column] public EntityType EntityType { get; set; }
    }
}
