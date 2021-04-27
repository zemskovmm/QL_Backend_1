using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using LinqToDB;

namespace QuartierLatin.Backend.Database.Repositories
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
    }
}