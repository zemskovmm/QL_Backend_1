using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.Cache;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Storages.Cache;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application
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

        public async Task DeleteGlobalSettingAsync(string key, int languageId)
        {
            var cacheKey = new GlobalSettingsCacheKey(key, languageId);

            await _globalSettingRepository.DeleteGlobalSettingAsync(key, languageId);

            await _globalSettingsCache.RemoveDataInCacheAsync(cacheKey);
        }
    }
}
