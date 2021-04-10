using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Models.Repositories;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageController(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        [AllowAnonymous]
        [HttpGet("/api/languages")]
        public async Task<IActionResult> GetLanguageList()
        {
            var languageList = await _languageRepository.GetLanguageListAsync();

            return Ok(languageList);
        }
    }
}
