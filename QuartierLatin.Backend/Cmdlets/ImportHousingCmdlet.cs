using CommandLine;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;
using QuartierLatin.Backend.Application.Infrastructure.Database;
using QuartierLatin.Backend.Application.Infrastructure.Database.AppDbContextSeed;
using QuartierLatin.Backend.Cmdlets.Utils;
using QuartierLatin.Backend.Config;
using QuartierLatin.Importer.DataModel.HousingImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Cmdlets
{
    public class ImportHousingCmdlet : CmdletBase<ImportHousingCmdlet.ImportHousingOptions>
    {
        private readonly AppDbContextManager _db;
        private readonly DatabaseConfig _dbConfig;
        private readonly ILanguageRepository _languageRepository;
        private readonly Dictionary<int, string> _language;

        [Verb("import-housing")]
        public class ImportHousingOptions
        {
            [Value(0)] public string JsonPath { get; set; }
        }

        public ImportHousingCmdlet(AppDbContextManager db, IConfiguration config, ILanguageRepository languageRepository)
        {
            _db = db;
            _dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
            _languageRepository = languageRepository;
            _language = _languageRepository.GetLanguageIdWithShortNameAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        protected override Task<int> Execute(ImportHousingOptions args) => _db.ExecAsync(async db =>
        {
            MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
            AppDbContextSeed.Seed(_db);

            Console.WriteLine("Truncate Housing, CommonTraitsToHousing, HousingGalleries, HousingLanguages tables ? \n Enter Y or N");
            var command = Console.ReadLine();

            if (command.ToLower() == "n")
                return 0;

            await db.HousingLanguages.TruncateAsync();
            await db.CommonTraitToHousings.TruncateAsync();
            await db.HousingGalleries.TruncateAsync();
            await db.ExecuteAsync("TRUNCATE TABLE \"Housings\" CASCADE");

            using var t = await db.BeginTransactionAsync();

            var import = JsonConvert.DeserializeObject<List<ImporterHousing>>(
                (await File.ReadAllTextAsync(args.JsonPath))
                // Some people aren't aware of ISO codes
                .Replace("\"ch\"", "\"cn\""));

            foreach (var housingImport in import)
            {
                var housingId = await CreateHousingAsync(db, housingImport.Price, housingImport.HousingLanguage);
                await CreateHousingAccommodationTypeAsync(db, housingId, housingImport.HousingAccommodation);
                await CreateHousingTraitsAsync(db, housingId, housingImport.CommonTraits);
                await CreateHousingGalleryAsync(db, housingId, housingImport.FileNames);
            }

            await t.CommitAsync();
            return 0;
        });

        private async Task<int> CreateHousingAsync(AppDbContext db, int? price, Dictionary<string, ImporterHousingLanguage> housingLanguageImport)
        {
            var housingId = await db.InsertWithInt32IdentityAsync(new Housing
            {
                Price = price
            });

            Console.WriteLine($"Adding housing wit id {housingId}");

            if (housingLanguageImport.Count == 0)
                return housingId;

            var housingLanguage = housingLanguageImport.Select(housingLanguageImport => new HousingLanguage
            {
                Description = housingLanguageImport.Value.Description,
                LanguageId = _language.FirstOrDefault(value => value.Value == housingLanguageImport.Key).Key,
                Name = string.IsNullOrEmpty(housingLanguageImport.Value.Name) ? "" : housingLanguageImport.Value.Name,
                Url = housingLanguageImport.Value.Url,
                HousingId = housingId
            }).ToList();

            await db.BulkCopyAsync(housingLanguage);

            return housingId;
        }

        private async Task CreateHousingAccommodationTypeAsync(AppDbContext db, int housingId, List<ImportHousingAccommodationType> housingAccommodationTypesImport)
        {
            if (housingAccommodationTypesImport.Count == 0)
                return;

            foreach (var housingAccommodationType in housingAccommodationTypesImport)
            {
                var housingAccommodationTypeId = await db.InsertWithInt32IdentityAsync(new HousingAccommodationType
                {
                    HousingId = housingId,
                    Names = housingAccommodationType.Names,
                    Price = housingAccommodationType.Price,
                    Residents = housingAccommodationType.Residents,
                    Square = housingAccommodationType.Square
                });

                Console.WriteLine($"Adding {housingAccommodationTypeId} HousingAccommodationType");

                await CreateHousingAccommodationTypeTraitsAsync(db, housingAccommodationTypeId,
                    housingAccommodationType.CommonTraits);
            }
        }

        private async Task CreateHousingAccommodationTypeTraitsAsync(AppDbContext db, int housingAccommodationTypeId,
            List<ImporterCommonTraits> commonTraits)
        {
            if (commonTraits.Count == 0)
                return;

            var traitsIds = await CreateTraitsAsync(db, commonTraits);

            var housingAccommodationTypeTraits = traitsIds.Select(traitId => new CommonTraitToHousingAccommodationType
            {
                CommonTraitId = traitId,
                HousingAccommodationTypeId = housingAccommodationTypeId
            });

            await db.BulkCopyAsync(housingAccommodationTypeTraits);
        }

        private async Task CreateHousingTraitsAsync(AppDbContext db, int housingId,
            List<ImporterCommonTraits> commonTraits)
        {
            if (commonTraits.Count == 0)
                return;

            var traitsIds = await CreateTraitsAsync(db, commonTraits);

            var housingTraits = traitsIds.Select(traitId => new CommonTraitToHousing
            {
                CommonTraitId = traitId,
                HousingId = housingId
            });

            await db.BulkCopyAsync(housingTraits);
        }

        private async Task<List<int>> CreateTraitsAsync(AppDbContext db, List<ImporterCommonTraits> commonTraits)
        {
            var response = new List<int>();

            foreach (var commonTrait in commonTraits)
            {
                if (commonTrait.Names.All(trait => string.IsNullOrEmpty(trait.Value)))
                    continue;

                if (string.IsNullOrEmpty(commonTrait.Identifier))
                    continue;

                var traitTypeNames = _language.ToDictionary(lang => lang.Value, lang => commonTrait.CommonTraitTypeName);

                var traitTypeId =
                    await ImportTraitHelper.EnsureTraitType(db, commonTrait.CommonTraitTypeName.ToLower(), traitTypeNames, EntityType.Housing);

                int? iconId = null;

                if (!string.IsNullOrEmpty(commonTrait.IconBlobFileName))
                    iconId = await CreateBlobAsync(db, commonTrait.IconBlobFileName);

                var commonTraitId = await ImportTraitHelper.EnsureTraitWithIdentifier(db, traitTypeId,
                    commonTrait.Identifier.ToLower(), commonTrait.Names, commonTrait.Order, iconId);

                response.Add(commonTraitId);
            }

            return response;
        }

        private async Task<int> CreateBlobAsync(AppDbContext db, string filename)
        {
            var blob = await db.Blobs.FirstOrDefaultAsync(blob => blob.OriginalFileName == filename);

            if (blob is not null)
                return blob.Id;

            blob = new Blob
            {
                FileType = "image",
                IsDeleted = false,
                OriginalFileName = filename
            };

            var blobId = await db.InsertWithInt32IdentityAsync(blob);

            Console.WriteLine($"Adding {blob.OriginalFileName} file with blob Id {blobId}");

            return blobId;
        }

        private async Task CreateHousingGalleryAsync(AppDbContext db, int housingId, List<string> fileNames)
        {
            if(fileNames.Count == 0)
                return;

            var blobsIds = new List<int>();

            foreach (var fileName in fileNames)
            {
                var blobId = await CreateBlobAsync(db, fileName);

                blobsIds.Add(blobId);
            }

            var housingGallery = blobsIds.Select(blobId => new HousingGallery
            {
                HousingId = housingId,
                ImageId = blobId
            });

            await db.BulkCopyAsync(housingGallery);
        }
    }
}
