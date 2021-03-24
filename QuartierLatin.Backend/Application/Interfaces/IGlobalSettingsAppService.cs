using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IGlobalSettingsAppService
    {
        Task<JObject> GetGlobalSettingAsync(string key, int languageId);

        Task CreateOrUpdateGlobalSettingAsync(string key, int languageId, JObject jsonData);

        Task DeleteGlobalSettingAsync(string key, int languageId);
    }
}
