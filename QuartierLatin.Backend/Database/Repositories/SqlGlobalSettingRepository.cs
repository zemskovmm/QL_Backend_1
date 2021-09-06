using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlGlobalSettingRepository : IGlobalSettingRepository
    {
        private readonly AppDbContextManager _db;

        public SqlGlobalSettingRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<JObject> GetGlobalSettingAsync(string key, int languageId)
        {
            return await _db.ExecAsync(async db =>
                {
                    var globalSettings = await
                        db.GlobalSettings.FirstOrDefaultAsync(setting =>
                            setting.Key == key && setting.LanguageId == languageId);

                    return globalSettings == null ? null : JObject.Parse(globalSettings.JsonData);
                }
            );
        }

        public async Task CreateOrUpdateGlobalSettingAsync(string key, int languageId, JObject jsonData)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new GlobalSetting
            {
                Key = key,
                LanguageId = languageId,
                JsonData = jsonData.ToString(Formatting.None)
            }));
        }

        public async Task<bool> DeleteGlobalSettingAsync(string key, int languageId)
        {
            return await _db.ExecAsync(async db =>
                {
                    var globalSettings =
                        db.GlobalSettings.Where(setting => setting.Key == key && setting.LanguageId == languageId);

                    if (!globalSettings.Any())
                        return false;

                    await globalSettings.DeleteAsync();
                    return true;
                }
            );
        }
    }
}