using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IDegreeRepository
    {
        Task<List<(Degree degree, int costGroup)>> GetDegreesForUniversity(int universityId);
        Task<List<Degree>> GetAll();
    }
}