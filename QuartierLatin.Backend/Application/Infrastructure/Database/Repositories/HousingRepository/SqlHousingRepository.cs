using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.HousingRepositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.HousingRepository
{
    public class SqlHousingRepository : IHousingRepository
    {
        private readonly AppDbContextManager _db;

        public SqlHousingRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateHousingAsync(int? price, int? imageId, List<HousingLanguage> housingLanguage)
        {
            return await _db.ExecAsync(db => db.InTransaction(async () =>
            {
                var housingId = await db.InsertWithInt32IdentityAsync(new Housing
                {
                    Price = price,
                    ImageId = imageId
                });

                housingLanguage.ForEach(courseLang => courseLang.HousingId = housingId);
                await db.BulkCopyAsync(housingLanguage);

                return housingId;
            }));
        }

        public async Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByIdAsync(int id)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = HousingWithLanguages(db, housing => housing.Id == id).First();

                return (housing: entity.Housing, housingLanguage: entity.HousingLanguage);
            });
        }

        public async Task<List<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)>> GetHousingListAsync()
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = HousingWithLanguages(db).ToList();

                var response = entity.Select(resp => (resp.Housing, resp.HousingLanguage)).ToList();

                return response;
            });
        }

        public async Task UpdateHousingByIdAsync(int id, int? price, int? imageId)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new Housing
            {
                Id = id,
                Price = price,
                ImageId = imageId
            }));
        }

        public async Task CreateOrUpdateHousingLanguageByIdAsync(int housingId, string htmlDescription, int languageId, string name, string url,
            JObject metadata)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new HousingLanguage
            {
                HousingId = housingId,
                LanguageId = languageId,
                Description = htmlDescription,
                Name = name,
                Url = url,
                Metadata = metadata?.ToString()
            }));
        }

        public async Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByUrlWithLanguageAsync(int languageId, string url)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = HousingWithLanguages(db,
                    housingLanguageFilter: housingLang => housingLang.LanguageId == languageId && housingLang.Url == url).First();

                return (housing: entity.Housing, schoolLanguage: entity.HousingLanguage);
            });
        }

        public async Task<(int totalItems, List<(Housing housing, HousingLanguage housingLanguage)> housingAndLanguage)> GetHousingPageByFilter(List<List<int>> commonTraitsIds, int langId, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var housings = db.Housings.AsQueryable();

                if (commonTraitsIds.Any())
                {
                    var housingWithTraits = db.CommonTraitToHousings.AsQueryable();

                    foreach (var commonTraitGroup in commonTraitsIds)
                    {
                        if (commonTraitGroup.Count != 0)
                            housingWithTraits =
                                housingWithTraits.Where(t => commonTraitGroup.Contains(t.CommonTraitId));
                    }

                    housings = housings.Where(housing =>
                        housingWithTraits.Select(x => x.HousingId).Contains(housing.Id));
                }

                var housingWithLanguages = from housing in housings
                                           join lang in db.HousingLanguages.Where(l => l.LanguageId == langId)
                                              on housing.Id equals lang.HousingId
                                          select new
                                          {
                                              housing,
                                              lang
                                          };

                var totalCount = await housingWithLanguages.CountAsync();

                return (totalCount,
                    (await housingWithLanguages.OrderBy(x => x.housing.Id).Skip(skip).Take(take).ToListAsync())
                    .Select(x => (housing: x.housing, housingLanguage: x.lang)).ToList());
            });
        }

        private record HousingAndLanguageTuple
        {
            public Housing Housing { get; set; }
            public Dictionary<int, HousingLanguage> HousingLanguage { get; set; }

            public void Deconstruct(out Housing housing, out Dictionary<int, HousingLanguage> housingLanguage)
            {
                housing = Housing;
                housingLanguage = HousingLanguage;
            }
        }

        private List<HousingAndLanguageTuple> HousingWithLanguages(AppDbContext db, Expression<Func<Housing, bool>> housingFilter = null, Expression<Func<HousingLanguage, bool>> housingLanguageFilter = null)
        {
            var housingQuery = db.Housings.AsQueryable();
            var housingLanguageQuery = db.HousingLanguages.AsQueryable();

            if (housingFilter is not null)
                housingQuery = housingQuery.Where(housingFilter);

            if (housingLanguageFilter is not null)
                housingLanguageQuery = housingLanguageQuery.Where(housingLanguageFilter);

            var query = from c in housingQuery
                        let langs = housingLanguageQuery.Where(lang => lang.HousingId == c.Id)
                select new { c, langs };

            var response = query.AsEnumerable().Select(q => new HousingAndLanguageTuple
            {
                Housing = q.c,
                HousingLanguage = q.langs.ToDictionary(lang => lang.LanguageId, lang => lang)
            }).ToList();

            return response;
        }
    }
}
