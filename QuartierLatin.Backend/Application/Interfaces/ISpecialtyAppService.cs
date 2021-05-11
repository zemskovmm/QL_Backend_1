using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface ISpecialtyAppService
    {
        Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId);
        Task<List<SpecialtyCategory>> GetSpecialCategoriesList();
    }
}
