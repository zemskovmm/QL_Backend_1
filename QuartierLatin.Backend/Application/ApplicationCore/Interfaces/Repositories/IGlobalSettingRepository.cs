using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface IGlobalSettingRepository
    {
        Task<JObject> GetGlobalSettingAsync(string key, int languageId);

        Task CreateOrUpdateGlobalSettingAsync(string key, int languageId, JObject jsonData);

        Task<bool> DeleteGlobalSettingAsync(string key, int languageId);
    }
}
