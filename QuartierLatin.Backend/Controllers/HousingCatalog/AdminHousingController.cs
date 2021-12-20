using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using QuartierLatin.Backend.Dto.HousingCatalogDto;
using RemoteUi;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;
using QuartierLatin.Backend.Dto.HousingCatalogDto.HousingAccommodationTypeCatalogDto;

namespace QuartierLatin.Backend.Controllers.HousingCatalog
{
    [Route("/api/admin/housings")]
    public class AdminHousingController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IHousingAppService _housingAppService;
        private readonly IHousingAccommodationTypeAppService _housingAccommodationTypeAppService;
        private readonly JObject _definition;

        public AdminHousingController(IHousingAppService housingAppService, ILanguageRepository languageRepository, IHousingAccommodationTypeAppService housingAccommodationTypeAppService)
        {
            var noFields = new IExtraRemoteUiField[0];
            _definition = new RemoteUiBuilder(typeof(HousingAdminDto), noFields, null, new CamelCaseNamingStrategy())
                .Register(typeof(HousingAdminDto), noFields)
                .Register(typeof(Dictionary<string, HousingLanguageAdminDto>), noFields)
                .Register(typeof(HousingLanguageAdminDto), noFields)
                .Build(null);

            _housingAppService = housingAppService;
            _languageRepository = languageRepository;
            _housingAccommodationTypeAppService = housingAccommodationTypeAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHousing()
        {
            var housingList = await _housingAppService.GetHousingListAsync();
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = housingList.Select(housing => new HousingListAdminDto()
            {
                Id = housing.housing.Id,
                Price = housing.housing.Price,
                Languages = housing.housingLanguage.ToDictionary(housing => language[housing.Key],
                    housing => new HousingLanguageAdminDto()
                    {
                        Name = housing.Value.Name,
                        HtmlDescription = housing.Value.Description,
                        Url = housing.Value.Url,
                        Metadata = housing.Value.Metadata is null ? null : JObject.Parse(housing.Value.Metadata),
                        Location = housing.Value.Location is null ? null : new HousingLanguageLocationAdminDto() {
							Lat=JObject.Parse(housing.Value.Location).SelectToken("lat").ToString(),

							Lng=JObject.Parse(housing.Value.Location).SelectToken("lng").ToString(),
							Address=JObject.Parse(housing.Value.Location).SelectToken("address").ToString()
							
						}
                    })
            }).ToList();

            return Ok(response);
        }

        [HttpGet("definition")]
        public IActionResult GetDefinition() => Ok(_definition);

        [HttpPost]
        public async Task<IActionResult> CreateHousing([FromBody] HousingAdminDto housingDto)
        {
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();   
            var housingLanguage = housingDto.Languages.Select(housing => new HousingLanguage
            {
                Description = housing.Value.HtmlDescription,
                Name = housing.Value.Name,
                Url = housing.Value.Url,
                LanguageId = language.FirstOrDefault(language => language.Value == housing.Key).Key,
                Metadata = housing.Value.Metadata is null ? null : housing.Value.Metadata.ToString(),
                Location =  housing.Value.Location is null ? null : housing.Value.Location.ToString(),
            }).ToList();

            var housingId = await _housingAppService.CreateHousingAsync(housingDto.Price, housingDto.ImageId, housingLanguage);

            return Ok(new { id = housingId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHousingById(int id)
        {
            var housing = await _housingAppService.GetHousingByIdAsync(id);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = new HousingAdminDto
            {
                Id = housing.housing.Id,
                Price = housing.housing.Price,
                Languages = housing.housingLanguage.ToDictionary(housing => language[housing.Key],
                    housing => new HousingLanguageAdminDto
                    {
                        Name = housing.Value.Name,
                        HtmlDescription = housing.Value.Description,
                        Url = housing.Value.Url,
                        Metadata = housing.Value.Metadata is null ? null : JObject.Parse(housing.Value.Metadata),
                        Location = housing.Value.Location is null ? null : new HousingLanguageLocationAdminDto() {
							Lat=JObject.Parse(housing.Value.Location)?.SelectToken("lat")?.ToString(),
							Lng=JObject.Parse(housing.Value.Location)?.SelectToken("lng")?.ToString(),
							Address=JObject.Parse(housing.Value.Location)?.SelectToken("address")?.ToString()
							
						}
                    })
            };

            return Ok(new AdminDtoResponse<HousingAdminDto> { Definition = _definition, Value = response });
        }
        
        [HttpGet("{id}/accommodations")]
        public async Task<IActionResult> GetAccommodationsByHousingId(int id)
        {
            var accommodations =
                await _housingAccommodationTypeAppService.GetHousingAccommodationTypeListByHousingIdAsync(id);
            
            var response = accommodations.Select(housing => new AdminHousingAccommodationTypeDto
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHousingById([FromBody] HousingAdminDto housingDto, int id)
        {
            await _housingAppService.UpdateHousingByIdAsync(id, housingDto.Price, housingDto.ImageId);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            foreach (var housingLanguage in housingDto.Languages)
            {
                var languageId = language.FirstOrDefault(language => language.Value == housingLanguage.Key).Key;
                await _housingAppService.CreateOrUpdateHousingLanguageByIdAsync(id,
                    housingLanguage.Value.HtmlDescription,
                    languageId,
                    housingLanguage.Value.Name,
                    housingLanguage.Value.Url, housingLanguage.Value.Metadata, new JObject
                       {
                             ["lat"] = housingLanguage.Value.Location.Lat,
                             ["lng"] = housingLanguage.Value.Location.Lng,
                             ["address"] = housingLanguage.Value.Location.Address
					   }
				);
            }

            return Ok(new object());
        }
    }
}
