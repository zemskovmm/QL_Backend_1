using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<Specialty> GetSpecialtyById(int specialtyId);
        Task<List<SpecialtyCategory>> GetSpecialtyCategoryList();
        Task<List<(Specialty, int, int)>> GetSpecialtiesUniversityByUniversityIdList(int universityId);
    }
}
