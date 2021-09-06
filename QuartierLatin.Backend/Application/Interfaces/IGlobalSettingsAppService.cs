using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IGlobalSettingsAppService
    {
        Task<JObject> GetGlobalSettingAsync(string key, int languageId);

        Task CreateOrUpdateGlobalSettingAsync(string key, int languageId, JObject jsonData);

        Task<bool> DeleteGlobalSettingAsync(string key, int languageId);
    }
}
