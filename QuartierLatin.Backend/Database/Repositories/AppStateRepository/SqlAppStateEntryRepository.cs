using LinqToDB;
using QuartierLatin.Backend.Models.AppStateModels;
using QuartierLatin.Backend.Models.Repositories.AppStateRepository;
using System;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.AppStateRepository
{
    public class SqlAppStateEntryRepository : IAppStateEntryRepository
    {
        private readonly AppDbContextManager _db;

        public SqlAppStateEntryRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<(DateTime lastUpdate, DateTime lastChange)> GetLastUpdateAndLastChangeDatesAsync()
        {
            var lastUpdatetime = await _db.ExecAsync(db => db.AppStateEntries.FirstOrDefaultAsync(state => state.Key == "LastUpdate"));
            var lastChangetime = await _db.ExecAsync(db => db.AppStateEntries.FirstOrDefaultAsync(state => state.Key == "LastChange"));

            return (lastUpdate: DateTime.Parse(lastUpdatetime.Value), lastChange: DateTime.Parse(lastChangetime.Value));
        }

        public async Task UpdateLastChangeTimeAsync()
        {
           await UpdateValueAsync("LastChange", DateTime.Now.ToString());
        }

        public async Task UpdateValueAsync(string key, string value)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new AppStateEntry
            {
                Key = key, 
                Value = value
            }));
        }
    }
}
