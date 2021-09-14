using CommandLine;
using Microsoft.Extensions.Configuration;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.Infrastructure.Database;
using QuartierLatin.Backend.Config;
using System;
using System.IO;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.Infrastructure.Database.AppDbContextSeed;
using System.Linq;

namespace QuartierLatin.Backend.Cmdlets
{
    public class ImportHousingFileRename : CmdletBase<ImportHousingFileRename.ImportHousingFileRenameOptions>
    {
        private readonly AppDbContextManager _db;
        private readonly DatabaseConfig _dbConfig;
        private readonly BlobConfig _blobConfig;

        [Verb("import-housing-blob-rename")]
        public class ImportHousingFileRenameOptions{}

        public ImportHousingFileRename(AppDbContextManager db, IConfiguration config, ILanguageRepository languageRepository)
        {
            _db = db;
            _dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
            _blobConfig = config.GetSection("Blob").Get<BlobConfig>();
        }

        protected override async Task<int> Execute(ImportHousingFileRenameOptions args) => await _db.ExecAsync(async db =>
        {
            MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
            AppDbContextSeed.Seed(_db);

            Console.WriteLine("Rename imported files in folder " + _blobConfig.Local.Path + " ? \n Enter Y or N");
            var command = Console.ReadLine();

            if (command.ToLower() == "n")
                return 0;

            var blobs = db.Blobs.AsAsyncEnumerable();

            await foreach (var blob in blobs)
            {
                var oldPath = Path.Combine(_blobConfig.Local.Path, blob.OriginalFileName);

                if(!File.Exists(oldPath))
                    continue;

                var newPath = GetPath(blob.Id, true);
                Console.WriteLine(oldPath + " NewPath: " + newPath);
                File.Move(oldPath, newPath);
            }

            return 0;
        });

        private string GetPath(int id, bool create)
        {
            var s = id.ToString();
            if (s.Length == 0)
                return Path.Combine(_blobConfig.Local.Path, s + ".bin");

            var ds = s.Substring(0, s.Length - 1);
            var dir = Path.Combine(ds.Select(x => x.ToString()).Prepend(_blobConfig.Local.Path).ToArray());
            if (create)
                Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, s.Last() + ".bin");

            return path;
        }
    }
}
