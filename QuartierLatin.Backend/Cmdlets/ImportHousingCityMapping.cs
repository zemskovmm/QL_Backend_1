using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommandLine;
using LinqToDB;
using Microsoft.Extensions.Configuration;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.Infrastructure.Database;
using QuartierLatin.Backend.Application.Infrastructure.Database.AppDbContextSeed;
using QuartierLatin.Backend.Config;

namespace QuartierLatin.Backend.Cmdlets
{
    public record OldCity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
    public record Housings
    {
        public string Name { get; set; }
    
        public int CityId { get; set; }
    }
    public record Dump
    {
        public List<OldCity> Cities { get; set; }
        public List<Housings> Housings { get; set; }
    }
    
    public class ImportHousingCityMapping : CmdletBase<ImportHousingCityMapping.ImportHousingCityMappingConfig>
    {
        private readonly AppDbContextManager _db;
        private readonly DatabaseConfig _dbConfig;
        private readonly Dictionary<int, string> _language;

        [Verb("housing-citymap")]
        public class ImportHousingCityMappingConfig
        {
            [Value(0)] public string JsonPath { get; set; }
        }

        
        public ImportHousingCityMapping(AppDbContextManager db, IConfiguration config, ILanguageRepository languageRepository)
        {
            _db = db;
            _dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
            _language = languageRepository.GetLanguageIdWithShortNameAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private CommonTrait OldCityToCommonTrait(OldCity x) => new ()
        {
            Identifier = x.Name,
            Names = _language.ToDictionary(d => d.Value, d => x.Name),
            CommonTraitTypeId = 2,
            Order = 0
        };
        
        protected override async Task<int> Execute(ImportHousingCityMappingConfig args)
        {
            MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
            AppDbContextSeed.Seed(_db);
            
            Console.WriteLine("Reading json file");
            var dump = JsonSerializer.Deserialize<Dump>(await File.ReadAllTextAsync(args.JsonPath));

            await _db.WithTransaction(async db =>
            {
                var dbCities = await db.CommonTraits.Where(x => x.CommonTraitTypeId == 2).ToListAsync();
                // OldId -> NewId
                var oldCityToCityMap = new Dictionary<int, int>();
                dump.Cities.ForEach(x =>
                {
                    var city = dbCities.FirstOrDefault(c => c.Names.ContainsValue(x.Name));
                    if (city is not null)
                    {
                        oldCityToCityMap[x.Id] = city.Id;
                        return;
                    }
                    
                    Console.WriteLine($"Missing {x.Name} at database! Inserting...");
                    var newCity = OldCityToCommonTrait(x);
                    var id = db.InsertWithInt32Identity(newCity);
                    newCity.Id = id;
                    dbCities.Add(newCity);
                    oldCityToCityMap[x.Id] = id;
                });
                
                Console.WriteLine("Creating parent links between CommonTraits");
                dbCities.ForEach(x =>
                {
                    var oldCity = dump.Cities.FirstOrDefault(c => oldCityToCityMap[c.Id] == x.Id);
                    if (oldCity is null || oldCity.ParentId is 0) return;
                    db.CommonTraits.Where(t => t.Id == x.Id)
                        .Set(t => t.ParentId, () => oldCityToCityMap[oldCity.ParentId])
                        .Update();
                });
                
                Console.WriteLine("Fixing missing housing cities");
                Console.WriteLine("TODO!");
            });
            return 0;
        }
    }
}