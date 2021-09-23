using CommandLine;
using QuartierLatin.Backend.Application.Infrastructure.Database;
using QuartierLatin.Backend.Config;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Infrastructure.Database.AppDbContextSeed;
using AsyncExtensions = LinqToDB.Async.AsyncExtensions;
using System.Linq;

namespace QuartierLatin.Backend.Cmdlets
{
    public class ImportHousingDescriptionRework : CmdletBase<ImportHousingDescriptionRework.ImportHousingDescriptionReworkOptions>
    {
        private readonly AppDbContextManager _db;
        private readonly DatabaseConfig _dbConfig;

        [Verb("import-housing-description-divide")]
        public class ImportHousingDescriptionReworkOptions { }

        public ImportHousingDescriptionRework(AppDbContextManager db, IConfiguration config)
        {
            _db = db;
            _dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
        }

        protected override async Task<int> Execute(ImportHousingDescriptionRework.ImportHousingDescriptionReworkOptions args) => await _db.ExecAsync(async db =>
        {
            MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
            AppDbContextSeed.Seed(_db);

            Console.WriteLine("Divide description field in housing language ? \n Enter Y or N");
            var command = Console.ReadLine();

            if (command.ToLower() == "n")
                return 0;

            var housingLanguage = await db.HousingLanguages.ToListAsync();

            foreach (var housingLang in housingLanguage)
            {
                try
                {
                    var json = JObject.Parse(Regex.Unescape(housingLang.Description));

                    var locationJObject = JObject.Parse(json["location"].ToString());

                    var desc = json["description"].ToString();

                    housingLang.Description = desc.Trim('\n', '\r');
                    housingLang.Location = locationJObject.ToString();

                    Console.WriteLine(housingLang.Description);
                    Console.WriteLine(housingLang.Location);

                    await db.UpdateAsync(housingLang);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
            }

            return 0;
        });

    }
}
