using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;

namespace QuartierLatin.Backend.Controllers
{
    public class GlobalSettingsController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IGlobalSettingsAppService _globalSettingsAppService;

        public GlobalSettingsController(ILanguageRepository languageRepository, IGlobalSettingsAppService globalSettingsAppService)
        {
            _languageRepository = languageRepository;
            _globalSettingsAppService = globalSettingsAppService;
        }

        [AllowAnonymous]
        [HttpGet("/api/global/{key}/{lang}"),
         ProducesResponseType(typeof(JObject), 200), 
         ProducesResponseType(404)]
        public async Task<IActionResult> GetGlobalSetting(string key, string lang)
        {
            lang = lang.ToLower();

            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);
            var result = await _globalSettingsAppService.GetGlobalSettingAsync(key, languageId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/api/admin/global/{key}/{lang}"),
         ProducesResponseType(200),
         ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGlobalSetting(string key, string lang)
        {
            lang = lang.ToLower();

            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);
            var response = await _globalSettingsAppService.DeleteGlobalSettingAsync(key, languageId);

            if (!response)
                return NotFound(new object());

            return Ok(new object());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/api/admin/global/{key}/{lang}"),
         ProducesResponseType(200)]
        public async Task<IActionResult> CreateOrUpdateGlobalSetting(string key, string lang, [FromBody] JObject jsonData)
        {
            lang = lang.ToLower();

            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);
            await _globalSettingsAppService.CreateOrUpdateGlobalSettingAsync(key, languageId, jsonData);

            return Ok(new object());
        }
    }
}
