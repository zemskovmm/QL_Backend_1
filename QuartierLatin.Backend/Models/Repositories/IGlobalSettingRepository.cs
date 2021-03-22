using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IGlobalSettingRepository
    {
        Task<JObject> GetGlobalSettingAsync(string key, int languageId);

        Task CreateOrUpdateGlobalSettingAsync(string key, int languageId, JObject jsonData);

        Task DeleteGlobalSettingAsync(string key, int languageId);
    }
}
