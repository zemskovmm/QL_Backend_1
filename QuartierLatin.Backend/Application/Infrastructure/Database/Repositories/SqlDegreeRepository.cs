using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories
{
    public class SqlDegreeRepository : IDegreeRepository
    {
        private readonly AppDbContextManager _db;

        public SqlDegreeRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public Task<List<(Degree degree, int costGroup)>> GetDegreesForUniversity(int universityId) =>
            _db.ExecAsync(async db => (await (from map in db.UniversityDegrees.Where(x => x.UniversityId == universityId)
                join degree in db.Degrees on map.DegreeId equals degree.Id
                select new {degree, map.CostGroup}).ToListAsync())
                .Select(x => (x.degree, x.CostGroup)).ToList());

        public Task<List<Degree>> GetAll() => _db.ExecAsync(db => db.Degrees.ToListAsync());

        public Task<Dictionary<int, List<Degree>>> GetDegreesForUniversities(List<int> universityIds) =>
            _db.ExecAsync(async db => ((await (from map in db.UniversityDegrees.Where(x => universityIds.Contains(x.UniversityId))
                    join degree in db.Degrees on map.DegreeId equals degree.Id
                    select new {map, degree}).ToListAsync())
                .GroupBy(x => x.map.UniversityId)
                .ToDictionary(x => x.Key, x => x.Select(d => d.degree).ToList())));
    }
}