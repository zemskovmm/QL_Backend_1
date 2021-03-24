using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.Repositories;
using System.Threading.Tasks;

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
        [HttpGet("/api/global/{key}/{lang}"), ProducesResponseType(typeof(JObject), 200)]
        public async Task<IActionResult> GetGlobalSetting(string key, string lang)
        {
            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);
            var result = await _globalSettingsAppService.GetGlobalSettingAsync(key, languageId);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/api/admin/global/{key}/{lang}")]
        public async Task<IActionResult> DeleteGlobalSetting(string key, string lang)
        {
            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);
            await _globalSettingsAppService.DeleteGlobalSettingAsync(key, languageId);

            return Ok(new object());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/api/admin/global/{key}/{lang}")]
        public async Task<IActionResult> CreateOrUpdateGlobalSetting(string key, string lang, [FromBody] JObject jsonData)
        {
            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);
            await _globalSettingsAppService.CreateOrUpdateGlobalSettingAsync(key, languageId, jsonData);

            return Ok(new object());
        }
    }
}
