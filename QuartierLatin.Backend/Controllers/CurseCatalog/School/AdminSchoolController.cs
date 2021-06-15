using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.courseCatalog.SchoolCatalog;
using QuartierLatin.Backend.Dto.courseCatalogDto.School;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels;

namespace QuartierLatin.Backend.Controllers.courseCatalog.School
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/schools")]
    public class AdminSchoolController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ISchoolAppService _schoolAppService;

        public AdminSchoolController(ILanguageRepository languageRepository, ISchoolAppService schoolAppService)
        {
            _languageRepository = languageRepository;
            _schoolAppService = schoolAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchool()
        {
            var schoolList = await _schoolAppService.GetSchoolListAsync();
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = schoolList.Select(school => new SchoolListAdminDto()
            {
                Id = school.school.Id,
                FoundationYear = school.school.FoundationYear,
                Languages = school.schoolLanguage.ToDictionary(school => language[school.Key],
                    school => new SchoolLanguageAdminDto()
                    {
                        Name = school.Value.Name,
                        HtmlDescription = school.Value.Description,
                        Url = school.Value.Url
                    })
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchool([FromBody] SchoolAdminDto schoolDto)
        {
            var schoolId = await _schoolAppService.CreateSchoolAsync(schoolDto.FoundationYear);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var schoolLanguage = schoolDto.Languages.Select(school => new SchoolLanguages
            {
                SchoolId = schoolId,
                Description = school.Value.HtmlDescription,
                Name = school.Value.Name,
                Url = school.Value.Url,
                LanguageId = language.FirstOrDefault(language => language.Value == school.Key).Key
            }).ToList();

            await _schoolAppService.CreateSchoolLanguageListAsync(schoolLanguage);

            return Ok(new { id = schoolId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GeSchoolById(int id)
        {
            var school = await _schoolAppService.GetSchoolByIdAsync(id);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = new SchoolAdminDto
            {
                FoundationYear = school.school.FoundationYear,
                Languages = school.schoolLanguage.ToDictionary(school => language[school.Key], 
                    school => new SchoolLanguageAdminDto
                    {
                        Name = school.Value.Name,
                        HtmlDescription = school.Value.Description,
                        Url = school.Value.Url
                    })
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolById([FromBody] SchoolAdminDto schoolDto, int id)
        {
            await _schoolAppService.UpdateSchoolByIdAsync(id, schoolDto.FoundationYear);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            foreach (var schoolLanguage in schoolDto.Languages)
            {
                var languageId = language.FirstOrDefault(language => language.Value == schoolLanguage.Key).Key;
                await _schoolAppService.UpdateSchoolLanguageByIdAsync(id,
                    schoolLanguage.Value.HtmlDescription,
                    languageId,
                    schoolLanguage.Value.Name,
                    schoolLanguage.Value.Url);
            }

            return Ok(new object());
        }
    }
}
