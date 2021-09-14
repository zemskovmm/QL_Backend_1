using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using QuartierLatin.Backend.Dto.HousingCatalogDto;
using QuartierLatin.Backend.Dto.HousingCatalogDto.HousingAccommodationTypeCatalogDto;
using RemoteUi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices;

namespace QuartierLatin.Backend.Controllers.HousingCatalog
{
    [Route("/api/housings/accommodation/type")]
    public class HousingAccommodationTypeController : Controller
    {
        private readonly IHousingAccommodationTypeAppService _housingAccommodationTypeAppService;
        private readonly JObject _definition;

        public HousingAccommodationTypeController(
            IHousingAccommodationTypeAppService housingAccommodationTypeAppService)
        {
            var noFields = new IExtraRemoteUiField[0];
            _definition = new RemoteUiBuilder(typeof(HousingAccommodationTypeDto), noFields, null, new CamelCaseNamingStrategy())
                .Build(null);

            _housingAccommodationTypeAppService = housingAccommodationTypeAppService;
        }

        [HttpGet("definition")]
        public IActionResult GetDefinition() => Ok(_definition);

        [HttpGet("housing/{id}")]
        public async Task<IActionResult> GetHousingAccommodationTypeListByHousingId(int id)
        {
            var housing = await _housingAccommodationTypeAppService.GetHousingAccommodationTypeListByHousingIdAsync(id);

            var response = housing.Select(housing => new HousingAccommodationTypeDto
            {
                Price = housing.Price,
                Square = housing.Square,
                Residents = housing.Residents,
                Names = housing.Names,
                HousingId = housing.HousingId
            }).ToList();

            return Ok(new AdminDtoResponse<List<HousingAccommodationTypeDto>> { Definition = _definition, Value = response });
        }
    }
}
