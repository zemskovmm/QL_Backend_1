using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;

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

        public async Task<List<(Specialty, int, int)>> GetSpecialtiesUniversityByUniversityIdList(int universityId)
        {
            var response = new List<(Specialty, int, int)>();

            var universitySpecialty = await _db.ExecAsync(db =>
                db.UniversitySpecialties.Where(speciallty => speciallty.UniversityId == universityId).ToListAsync());

            foreach (var specialty in universitySpecialty)
            {
                var specialtyEntity = await GetSpecialtyById(specialty.SpecialtyId);
                response.Add((specialtyEntity, specialty.CostFrom, specialty.CostTo));
            }

            return response;
        }
    }
}
