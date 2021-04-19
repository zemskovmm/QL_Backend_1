using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Database.Repositories.CatalogRepository
{
    public class SqlUniversityRepository : IUniversityRepository
    {
        private readonly AppDbContextManager _db;

        public SqlUniversityRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task CreateOrUpdateUniversityLanguageAsync(int universityId, int languageId, string name,
            string description, string url)
        {
            await _db.ExecAsync(
                db => CreateOrUpdateUniversityCore(db, universityId, languageId, name, description, url));
        }

        public async Task<List<int>> GetUniversityIdListAsync()
        {
            return await _db.ExecAsync(db => db.Universities.Select(university => university.Id).ToListAsync());
        }

        public async Task<Dictionary<int, UniversityLanguage>> GetUniversityLanguageByUniversityIdAsync(
            int universityId)
        {
            return await _db.ExecAsync(db => db.UniversityLanguages
                .Where(universityLanguage => universityLanguage.UniversityId == universityId)
                .ToDictionaryAsync(universityLanguage => universityLanguage.LanguageId,
                    universityLanguage => universityLanguage));
        }

        public async Task<University> GetUniversityByIdAsync(int id)
        {
            return await _db.ExecAsync(db => db.Universities.FirstOrDefaultAsync(university => university.Id == id));
        }

        public async Task CreateUniversityLanguageListAsync(List<UniversityLanguage> universityLanguage)
        {
            await _db.ExecAsync(db => db.BulkCopyAsync(universityLanguage));
        }

        public async Task<int> CreateUniversityAsync(int? foundationYear, string website)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new University
            {
                FoundationYear = foundationYear,
                Website = website
            }));
        }

        public async Task UpdateUniversityAsync(int id, int? foundationYear, string website)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new University
            {
                Id = id,
                FoundationYear = foundationYear,
                Website = website
            }));
        }

        public async Task<int> GetUniversityIdByUrl(string url)
        {
            return await _db.ExecAsync(db =>
                db.UniversityLanguages.Where(university => university.Url == url).Select(university => university.UniversityId).FirstAsync());
        }

        public async Task<List<UniversityInstructionLanguage>> GetUniversityLanguageInstructionByUniversityId(int universityId)
        {
            return await _db.ExecAsync(db =>
                db.UniversityInstructionLanguages.Where(instruction => instruction.UniversityId == universityId)
                    .ToListAsync());
        }

        public async Task<List<(Specialty, int)>> GetSpecialtiesUniversityByUniversityIdList(int universityId)
        {
            var response = new List<(Specialty, int)>();

            var universitySpecialty = await _db.ExecAsync(db =>
                db.UniversitySpecialties.Where(speciallty => speciallty.UniversityId == universityId).ToListAsync());

            foreach (var specialty in universitySpecialty)
            {
                var specialtyEntity = await GetSpecialtyById(specialty.SpecialtyId);
                response.Add((specialtyEntity, specialty.Cost));
            }

            return response;
        }

        public async Task<Specialty> GetSpecialtyById(int specialtyId)
        {
            return await _db.ExecAsync(db => db.Specialties.FirstOrDefaultAsync(specialty => specialty.Id == specialtyId));
        }

        public async Task<int> GetUniversityIdByUrlAndLanguage(int languageId, string url)
        {
            return await _db.ExecAsync(db =>
                db.UniversityLanguages.Where(university => university.Url == url && university.LanguageId == languageId)
                    .Select(university => university.UniversityId).FirstAsync());
        }

        private static async Task CreateOrUpdateUniversityCore(AppDbContext db, int universityId, int languageId,
            string name, string description, string url)
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