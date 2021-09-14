using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Cache;
using QuartierLatin.Backend.Storages.Cache;

namespace QuartierLatin.Backend.Services
{
    public class GlobalSettingsAppService : IGlobalSettingsAppService
    {
        private readonly IGlobalSettingRepository _globalSettingRepository;
        private readonly GlobalSettingsCache<JObject> _globalSettingsCache;

        public GlobalSettingsAppService(IGlobalSettingRepository globalSettingRepository, GlobalSettingsCache<JObject> globalSettingsCache)
        {
            _globalSettingRepository = globalSettingRepository;
            _globalSettingsCache = globalSettingsCache;
        }
        public async Task<JObject> GetGlobalSettingAsync(string key, int languageId)
        {
            var cacheKey = new GlobalSettingsCacheKey(key, languageId);

            return await _globalSettingsCache.GetOrCreateAsync(cacheKey,
                async () => await _globalSettingRepository.GetGlobalSettingAsync(key, languageId));
        }

        public async Task CreateOrUpdateGlobalSettingAsync(string key, int languageId, JObject jsonData)
        {
            var cacheKey = new GlobalSettingsCacheKey(key, languageId);
            await _globalSettingRepository.CreateOrUpdateGlobalSettingAsync(key, languageId, jsonData);
            await _globalSettingsCache.UpdateDataInCacheAsync(cacheKey,
                async () => await _globalSettingRepository.GetGlobalSettingAsync(key, languageId));
        }

        public async Task<bool> DeleteGlobalSettingAsync(string key, int languageId)
        {
            var cacheKey = new GlobalSettingsCacheKey(key, languageId);

            var response = await _globalSettingRepository.DeleteGlobalSettingAsync(key, languageId);

            if (!response)
                return response;

            await _globalSettingsCache.RemoveDataInCacheAsync(cacheKey);

            return response;
        }
    }
}
