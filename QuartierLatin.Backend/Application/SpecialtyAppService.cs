using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Application
{
    public class SpecialtyAppService : ISpecialtyAppService
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        public SpecialtyAppService(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public async Task<List<(Specialty, int, int)>> GetSpecialtiesUniversityByUniversityId(int universityId)
        {
            return await _specialtyRepository.GetSpecialtiesUniversityByUniversityIdList(universityId);
        }

        public async Task<List<SpecialtyCategory>> GetSpecialCategoriesList()
        {
            return await _specialtyRepository.GetSpecialtyCategoryList();
        }
    }
}
