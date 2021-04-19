using QuartierLatin.Backend.Models.CatalogModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICatalogAppService
    {
        Task<List<(CommonTraitType, List<CommonTrait>)>> GetNamedCommonTraitsAndTraitTypeByEntityType(EntityType entityType);
    }
}
