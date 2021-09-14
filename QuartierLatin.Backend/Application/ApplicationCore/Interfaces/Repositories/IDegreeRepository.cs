using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface IDegreeRepository
    {
        Task<List<(Degree degree, int costGroup)>> GetDegreesForUniversity(int universityId);
        Task<List<Degree>> GetAll();
        Task<Dictionary<int, List<Degree>>> GetDegreesForUniversities(List<int> universityIds);
    }
}