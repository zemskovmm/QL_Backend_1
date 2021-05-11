using LinqToDB;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlSpecialtyRepository : ISpecialtyRepository
    {
        private readonly AppDbContextManager _db;

        public SqlSpecialtyRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<SpecialtyCategory>> GetSpecialtyCategoryList()
        {
            return await _db.ExecAsync(db => db.SpecialtyCategories.ToListAsync());
        }

        public async Task<Specialty> GetSpecialtyById(int specialtyId)
        {
            return await _db.ExecAsync(db => db.Specialties.FirstOrDefaultAsync(specialty => specialty.Id == specialtyId));
        }

        public Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId) =>
            _db.ExecAsync(db => (from map in db.UniversitySpecialties.Where(x => x.UniversityId == universityId)
                join spec in db.Specialties on map.UniversityId equals spec.Id
                select spec).ToListAsync());
    }
}
