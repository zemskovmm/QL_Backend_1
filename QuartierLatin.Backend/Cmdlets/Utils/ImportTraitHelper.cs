using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Cmdlets.Utils
{
    public static class ImportTraitHelper
    {
        public static async Task<int> EnsureTraitType(AppDbContext db, string identifier, Dictionary<string, string> names, EntityType type = EntityType.University, int order = 0)
        {
            var traitType = db.CommonTraitTypes.FirstOrDefault(x => x.Identifier == identifier);
            if (traitType == null)
            {
                Console.WriteLine($"Adding {identifier} trait type");
                traitType = new CommonTraitType
                {
                    Identifier = identifier,
                    Names = names,
                    Order = order
                };
                traitType.Id = await db.InsertWithInt32IdentityAsync(traitType);
                await db.InsertAsync(new CommonTraitTypesForEntity
                {
                    EntityType = type,
                    CommonTraitId = traitType.Id
                });
            }

            return traitType.Id;
        }

        public static async Task<int> EnsureTraitWithIdentifier(AppDbContext db, int traitTypeId, string identifier,
            Dictionary<string, string> names, int order = 0, int? iconBlobId = null)
        {
            var trait = db.CommonTraits.FirstOrDefault(x =>
                x.CommonTraitTypeId == traitTypeId && x.Identifier == identifier);
            if (trait != null)
                return trait.Id;

            Console.WriteLine("Adding " + identifier);
            return await db.InsertWithInt32IdentityAsync(new CommonTrait
            {
                Identifier = identifier,
                Names = names,
                CommonTraitTypeId = traitTypeId,
                Order = order,
                IconBlobId = iconBlobId
            });
        }
    }
}
