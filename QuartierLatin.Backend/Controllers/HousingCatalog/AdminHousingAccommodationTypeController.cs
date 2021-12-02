using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using QuartierLatin.Backend.Dto.HousingCatalogDto;
using QuartierLatin.Backend.Dto.HousingCatalogDto.HousingAccommodationTypeCatalogDto;
using RemoteUi;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices;

namespace QuartierLatin.Backend.Controllers.HousingCatalog
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/housings/accommodation/type")]
    public class AdminHousingAccommodationTypeController : Controller
    {
        private readonly IHousingAccommodationTypeAppService _housingAccommodationTypeAppService;
        private readonly JObject _definition;

        public AdminHousingAccommodationTypeController(
            IHousingAccommodationTypeAppService housingAccommodationTypeAppService)
        {
            var noFields = new IExtraRemoteUiField[0];
            _definition = new RemoteUiBuilder(typeof(AdminHousingAccommodationTypeDto), noFields, null, new CamelCaseNamingStrategy())
                .Register(typeof(HousingAccommodationTypeDto), noFields)
                .Register(typeof(Dictionary<string, string>), noFields)
                .Build(null);

            _housingAccommodationTypeAppService = housingAccommodationTypeAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHousingAccommodationType()
        {
            var housingAccommodationTypeList = await _housingAccommodationTypeAppService.GetHousingAccommodationTypeListAsync();

            var response = housingAccommodationTypeList.Select(housing => new AdminHousingAccommodationTypeDto
            {
                Id = housing.Id,
                Price = housing.Price,
                Square = housing.Square,
                Residents = housing.Residents,
                Names = housing.Names,
                HousingId = housing.HousingId
            }).ToList();

            return Ok(response);
        }

        [HttpGet("definition")]
        public IActionResult GetDefinition() => Ok(_definition);

        [HttpPost]
        public async Task<IActionResult> CreateHousingAccommodationType([FromBody] AdminHousingAccommodationTypeDto housingDto)
        {
            var housingId = await _housingAccommodationTypeAppService.CreateHousingAccommodationTypeAsync(housingDto.Names, housingDto.HousingId, 
                housingDto.Price, housingDto.Residents, housingDto.Square);

            return Ok(new { id = housingId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHousingAccommodationTypeById(int id)
        {
            var housing = await _housingAccommodationTypeAppService.GetHousingAccommodationTypeByIdAsync(id);

            var response = new AdminHousingAccommodationTypeDto
            {
                Id = housing.Id,
                Price = housing.Price,
                Square = housing.Square,
                Residents = housing.Residents,
                Names = housing.Names,
                HousingId = housing.HousingId
            };

            return Ok(new AdminDtoResponse<AdminHousingAccommodationTypeDto> { Definition = _definition, Value = response });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHousingAccommodationTypeById([FromBody] AdminHousingAccommodationTypeDto housingDto, int id)
        {
            await _housingAccommodationTypeAppService.UpdateHousingAccommodationTypeByIdAsync(id, housingDto.Names, housingDto.HousingId,
                housingDto.Price, housingDto.Residents, housingDto.Square);
            
            return Ok(new object());
        }
    }
}
