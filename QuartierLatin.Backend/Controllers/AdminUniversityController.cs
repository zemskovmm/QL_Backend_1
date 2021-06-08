using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/universities")]
    public class AdminUniversityController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IUniversityAppService _universityAppService;

        public AdminUniversityController(IUniversityAppService universityAppService,
            ILanguageRepository languageRepository)
        {
            _universityAppService = universityAppService;
            _languageRepository = languageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversity()
        {
            var universityList = await _universityAppService.GetUniversityListAsync();

            var response = universityList.Select(university => new UniversityListDto
                {
                    Id = university.university.Id,
                    FoundationYear = university.university.FoundationYear,
                    MinimumAge = 18,
                    Website = "/",
                    Languages = university.universityLanguage.ToDictionary(university => _languageRepository
                        .GetLanguageShortNameAsync(university.Key)
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult(), university => new UniversityLanguageDto
                    {
                        Name = university.Value.Name,
                        HtmlDescription = university.Value.Description,
                        Url = university.Value.Url
                    })
                })
                .ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUniversity([FromBody] UniversityDto university)
        {
            var universityId =
                await _universityAppService.CreateUniversityAsync(university.FoundationYear);

            var universityLanguage = university.Languages.Select(university => new UniversityLanguage
            {
                UniversityId = universityId,
                Description = university.Value.HtmlDescription,
                Name = university.Value.Name,
                Url = university.Value.Url,
                LanguageId = _languageRepository
                    .GetLanguageIdByShortNameAsync(university.Key)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult()
            }).ToList();

            await _universityAppService.CreateUniversityLanguageListAsync(universityLanguage);

            return Ok(new {id = universityId});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversityById(int id)
        {
            var university = await _universityAppService.GetUniversityByIdAsync(id);

            var response = new UniversityDto
            {
                FoundationYear = university.university.FoundationYear,
                Languages = university.universityLanguage.ToDictionary(university => _languageRepository
                    .GetLanguageShortNameAsync(university.Key)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult(), university => new UniversityLanguageDto
                {
                    Name = university.Value.Name,
                    HtmlDescription = university.Value.Description,
                    Url = university.Value.Url
                })
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUniversityById([FromBody] UniversityDto universityDto, int id)
        {
            await _universityAppService.UpdateUniversityByIdAsync(id, universityDto.FoundationYear);

            foreach (var universityLanguage in universityDto.Languages)
            {
                var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(universityLanguage.Key);
                await _universityAppService.UpdateUniversityLanguageByIdAsync(id,
                    universityLanguage.Value.HtmlDescription,
                    languageId,
                    universityLanguage.Value.Name,
                    universityLanguage.Value.Url);
            }

            return Ok(new object());
        }
    }
}