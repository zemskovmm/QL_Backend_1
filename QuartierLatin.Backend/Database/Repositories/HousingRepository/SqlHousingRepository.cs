using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.HousingModels;
using QuartierLatin.Backend.Models.Repositories.HousingRepositories;

namespace QuartierLatin.Backend.Database.Repositories.HousingRepository
{
    public class SqlHousingRepository : IHousingRepository
    {
        private readonly AppDbContextManager _db;

        public SqlHousingRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateHousingAsync(int? price)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new Housing
            {
                Price = price,
            }));
        }

        public async Task CreateHousingLanguageListAsync(List<HousingLanguage> housingLanguage)
        {
            await _db.ExecAsync(db => db.BulkCopyAsync(housingLanguage));
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

        public async Task UpdateHousingByIdAsync(int id, int? price)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new Housing
            {
                Id = id,
                Price = price
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
