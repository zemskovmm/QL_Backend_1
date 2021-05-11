using QuartierLatin.Backend.Models.CatalogModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<Specialty> GetSpecialtyById(int specialtyId);
        Task<List<SpecialtyCategory>> GetSpecialtyCategoryList();
        Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId);
    }
}
