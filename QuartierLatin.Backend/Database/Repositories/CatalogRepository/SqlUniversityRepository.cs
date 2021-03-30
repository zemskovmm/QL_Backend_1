using LinqToDB;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.CatalogRepository
{
    public class SqlUniversityRepository : IUniversityRepository
    {
        private readonly AppDbContextManager _db;

        public SqlUniversityRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateUniversityLanguageAsync(int languageId, string name, string description, string url, string website, int foundationYear)
        {
            return await _db.ExecAsync(async db =>
            {
                await using var t = await db.BeginTransactionAsync();

                var universityId = await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new University
                {
                    FoundationYear = foundationYear,
                    Website = website
                }));

                await CreateOrUpdateUniversityCore(db, universityId, languageId, name, description, url);
                await t.CommitAsync();

                return universityId;
            });
        }

        public async Task CreateOrUpdateUniversityLanguageAsync(int universityId, int languageId, string name, string description, string url) =>
            await _db.ExecAsync(db => CreateOrUpdateUniversityCore(db, universityId, languageId, name, description, url));

        public async Task DeleteUniversityLanguageAsync(int universityId, int languageId)
        {
            await _db.ExecAsync(db => db.UniversityLanguages.Where(university =>
                university.UniversityId == universityId && university.LanguageId == languageId).DeleteAsync());
        }

        public async Task<(University, UniversityLanguage)> GetUniversityLanguageAsync(int universityId, int languageId)
        {
            var university = await _db.ExecAsync(db =>
                db.Universities.FirstOrDefaultAsync(university => university.Id == universityId));

            var universityLanguage = await _db.ExecAsync(db => db.UniversityLanguages.FirstOrDefaultAsync(university =>
                university.UniversityId == universityId && university.LanguageId == languageId));

            return (university, universityLanguage);
        }

        private static async Task CreateOrUpdateUniversityCore(AppDbContext db, int universityId, int languageId, string name, string description, string url)
        {
            await db.InsertOrReplaceAsync(new UniversityLanguage
            {
                UniversityId = universityId,
                LanguageId = languageId,
                Name = name,
                Description = description,
                Url = url
            });
        }
    }
}
