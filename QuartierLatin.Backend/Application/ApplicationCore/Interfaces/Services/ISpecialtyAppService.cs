using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services
{
    public interface ISpecialtyAppService
    {
        Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId);
        Task<List<SpecialtyCategory>> GetSpecialCategoriesList();
    }
}
