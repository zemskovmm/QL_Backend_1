using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.HousingRepositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.HousingRepository
{
    public class SqlHousingAccommodationTypeRepository : IHousingAccommodationTypeRepository
    {
        private readonly AppDbContextManager _db;

        public SqlHousingAccommodationTypeRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListAsync()
        {
            return await _db.ExecAsync(db => db.HousingAccommodationTypes.ToListAsync());
        }

        public async Task<int> CreateHousingAccommodationTypeAsync(Dictionary<string, string> names, int housingId, int price, string residents, string square)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new HousingAccommodationType
            {
                Names = names,
                HousingId = housingId,
                Price = price,
                Residents = residents,
                Square = square
            }));
        }

        public async Task<HousingAccommodationType> GetHousingAccommodationTypeByIdAsync(int id)
        {
            return await _db.ExecAsync(db => db.HousingAccommodationTypes.FirstOrDefaultAsync(housing => housing.Id == id));
        }

        public async Task UpdateHousingAccommodationTypeByIdAsync(int id, Dictionary<string, string> names, int housingId, int price, string residents, string square)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new HousingAccommodationType
            {
                Id = id,
                Names = names,
                HousingId = housingId,
                Price = price,
                Residents = residents,
                Square = square
            }));
        }

        public async Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListByHousingIdAsync(int housingId)
        {
            return await _db.ExecAsync(db => db.HousingAccommodationTypes.Where(housing => housing.HousingId == housingId).ToListAsync());
        }
    }
}
