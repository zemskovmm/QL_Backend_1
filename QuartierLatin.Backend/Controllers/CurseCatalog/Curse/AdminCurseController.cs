using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.CurseCatalog.CurseCatalog;
using QuartierLatin.Backend.Dto.CurseCatalogDto.Curse;
using QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers.CurseCatalog.Curse
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/curses")]
    public class AdminCurseController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ICurseAppService _curseAppService;

        public AdminCurseController(ILanguageRepository languageRepository, ICurseAppService curseAppService)
        {
            _languageRepository = languageRepository;
            _curseAppService = curseAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurse()
        {
            var curseList = await _curseAppService.GetCurseListAsync();
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = curseList.Select(curse => new CurseListAdminDto()
            {
                Id = curse.curse.Id,
                SchoolId = curse.curse.SchoolId,
                Languages = curse.curseLanguage.ToDictionary(school => language[school.Key],
                    school => new CurseLanguageAdminDto
                    {
                        Name = school.Value.Name,
                        HtmlDescription = school.Value.Description,
                        Url = school.Value.Url
                    })
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurse([FromBody] CurseAdminDto curseDto)
        {
            var curseId = await _curseAppService.CreateCurseAsync(curseDto.SchoolId);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var curseLanguage = curseDto.Languages.Select(curse => new CurseLanguage
            {
                CurseId = curseId,
                Description = curse.Value.HtmlDescription,
                Name = curse.Value.Name,
                Url = curse.Value.Url,
                LanguageId = language.FirstOrDefault(language => language.Value == curse.Key).Key
            }).ToList();

            await _curseAppService.CreateCurseLanguageListAsync(curseLanguage);

            return Ok(new { id = curseId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurseById(int id)
        {
            var curse = await _curseAppService.GetCurseByIdAsync(id);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var response = new CurseAdminDto
            {
                SchoolId = curse.curse.SchoolId,
                Languages = curse.schoolLanguage.ToDictionary(curse => language[curse.Key],
                    curse => new CurseLanguageAdminDto
                    {
                        Name = curse.Value.Name,
                        HtmlDescription = curse.Value.Description,
                        Url = curse.Value.Url
                    })
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurseById([FromBody] CurseAdminDto curseDto, int id)
        {
            await _curseAppService.UpdateCurseByIdAsync(id, curseDto.SchoolId);
            var language = await _languageRepository.GetLanguageIdWithShortNameAsync();

            foreach (var curseLanguage in curseDto.Languages)
            {
                var languageId = language.FirstOrDefault(language => language.Value == curseLanguage.Key).Key;
                await _curseAppService.UpdateCurseLanguageByIdAsync(id,
                    curseLanguage.Value.HtmlDescription,
                    languageId,
                    curseLanguage.Value.Name,
                    curseLanguage.Value.Url);
            }

            return Ok(new object());
        }
    }
}
