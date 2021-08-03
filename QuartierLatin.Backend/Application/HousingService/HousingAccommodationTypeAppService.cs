using QuartierLatin.Backend.Application.Interfaces.HousingServices;
using QuartierLatin.Backend.Models.Repositories.HousingRepositories;

namespace QuartierLatin.Backend.Application.HousingService
{
    public class HousingAccommodationTypeAppService : IHousingAccommodationTypeAppService
    {
        private readonly IHousingAccommodationTypeRepository _housingAccommodationTypeRepository;

        public HousingAccommodationTypeAppService(IHousingAccommodationTypeRepository housingAccommodationTypeRepository)
        {
            _housingAccommodationTypeRepository = housingAccommodationTypeRepository;
        }
    }
}
