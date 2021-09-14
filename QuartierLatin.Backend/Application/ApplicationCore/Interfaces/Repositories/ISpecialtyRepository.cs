using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<Specialty> GetSpecialtyById(int specialtyId);
        Task<List<SpecialtyCategory>> GetSpecialtyCategoryList();
        Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId);
    }
}
