using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using QuartierLatin.Backend.Application.Interfaces.HousingServices;
using QuartierLatin.Backend.Dto.HousingCatalogDto;
using QuartierLatin.Backend.Models.HousingModels;
using QuartierLatin.Backend.Models.Repositories;
using RemoteUi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers.HousingCatalog
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/housings")]
    public class AdminHousingController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IHousingAppService _housingAppService;
        private readonly JObject _definition;

        public AdminHousingController(IHousingAppService housingAppService, ILanguageRepository languageRepository)
        {
            var noFields = new IExtraRemoteUiField[0];
            _definition = new RemoteUiBuilder(typeof(HousingAdminDto), noFields, null, new CamelCaseNamingStrategy())
                .Register(typeof(HousingAdminDto), noFields)
                .Register(typeof(Dictionary<string, HousingLanguageAdminDto>), noFields)
                .Register(typeof(HousingLanguageAdminDto), noFields)
                .Build(null);

            _housingAppService = housingAppService;
            _languageRepository = languageRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetSchool()
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
                        Metadata = housing.Value.Metadata is null ? null : JObject.Parse(housing.Value.Metadata)
                    })
            }).ToList();

            return Ok(response);
        }

        [HttpGet("definition")]
        public IActionResult GetDefinition() => Ok(_definition);

        [HttpPost]
        public async Task<IActionResult> CreateSchool([FromBody] HousingAdminDto housingDto)
        {
            var housingId = await _housingAppService.CreateHousingAsync(housingDto.Price);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var housingLanguage = housingDto.Languages.Select(housing => new HousingLanguage
            {
                HousingId = housingId,
                Description = housing.Value.HtmlDescription,
                Name = housing.Value.Name,
                Url = housing.Value.Url,
                LanguageId = language.FirstOrDefault(language => language.Value == housing.Key).Key,
                Metadata = housing.Value.Metadata is null ? null : housing.Value.Metadata.ToString()
            }).ToList();

            await _housingAppService.CreateHousingLanguageListAsync(housingLanguage);

            return Ok(new { id = housingId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GeSchoolById(int id)
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
                        Metadata = housing.Value.Metadata is null ? null : JObject.Parse(housing.Value.Metadata)
                    })
            };

            return Ok(new AdminDtoResponse<HousingAdminDto> { Definition = _definition, Value = response });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolById([FromBody] HousingAdminDto housingDto, int id)
        {
            await _housingAppService.UpdateHousingByIdAsync(id, housingDto.Price);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            foreach (var housingLanguage in housingDto.Languages)
            {
                var languageId = language.FirstOrDefault(language => language.Value == housingLanguage.Key).Key;
                await _housingAppService.CreateOrUpdateHousingLanguageByIdAsync(id,
                    housingLanguage.Value.HtmlDescription,
                    languageId,
                    housingLanguage.Value.Name,
                    housingLanguage.Value.Url, housingLanguage.Value.Metadata);
            }

            return Ok(new object());
        }
    }
}
