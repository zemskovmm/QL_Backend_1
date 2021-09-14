using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Services
{
    public class SpecialtyAppService : ISpecialtyAppService
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        public SpecialtyAppService(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId) =>
            _specialtyRepository.GetSpecialtiesUniversityByUniversityId(universityId);

        public async Task<List<SpecialtyCategory>> GetSpecialCategoriesList()
        {
            return await _specialtyRepository.GetSpecialtyCategoryList();
        }
    }
}
