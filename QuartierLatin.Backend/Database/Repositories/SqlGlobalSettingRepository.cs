using LinqToDB;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlGlobalSettingRepository : IGlobalSettingRepository
    {
        private readonly AppDbContextManager _db;

        public SqlGlobalSettingRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<JObject> GetGlobalSettingAsync(string key, int languageId) =>
            await _db.ExecAsync(db =>
            db.GlobalSettings.Where(setting =>
                setting.Key == key && setting.LanguageId == languageId).Select(setting => JObject.Parse(setting.JsonData)).FirstAsync());


        public async Task CreateOrUpdateGlobalSettingAsync(string key, int languageId, JObject jsonData)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new GlobalSetting
                {
                    Key = key,
                    LanguageId = languageId,
                    JsonData = jsonData.ToString(Newtonsoft.Json.Formatting.None)
                }));
        }

        public async Task DeleteGlobalSettingAsync(string key, int languageId)
        {
            await _db.ExecAsync(db => db.GlobalSettings.Where(setting => setting.Key == key && setting.LanguageId == languageId).DeleteAsync());
        }
    }
}
