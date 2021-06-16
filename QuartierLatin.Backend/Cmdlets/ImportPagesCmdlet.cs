using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommandLine;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NickBuhro.Translit;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Database;
using QuartierLatin.Backend.Database.AppDbContextSeed;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.FolderModels;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Utils;
using QuartierLatin.Importer;
using QuartierLatin.Importer.DataModel;

namespace QuartierLatin.Backend.Cmdlets
{
    public class ImportPagesCmdlet : CmdletBase<ImportPagesCmdlet.ImportPagesCmdletOptions>
    {
        private readonly AppDbContextManager _db;
        private readonly IFileAppService _fileAppService;
        private readonly DatabaseConfig _dbConfig;

        [Verb("import-pages")]
        public class ImportPagesCmdletOptions
        {
            [Value(0)] public string Input { get; set; }
        }

        public ImportPagesCmdlet(AppDbContextManager db, IConfiguration config, IFileAppService fileAppService)
        {
            _db = db;
            _fileAppService = fileAppService;
            _dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
        }

        int EnsureLegacyImageUrl(string url)
        {
            return _db.Exec(db =>
            {
                var importedDir =
                    db.StorageFolders.Where(x => x.FolderParentId == null && x.FolderName == "<Imported>")
                        .Select(x => x.Id).FirstOrDefault();

                if (importedDir == 0)
                    importedDir = db.InsertWithInt32Identity(new StorageFolder
                    {
                        FolderName = "<Imported>",
                    });

                var existing =
                    db.Blobs.FirstOrDefault(b => b.OriginalFileName == url && b.StorageFolderId == importedDir);
                if (existing != null)
                    return existing.Id;

                Console.WriteLine("Downloading " + url);
                var data = new WebClient().DownloadData("https://quartier-latin.com" + url);
                return _fileAppService.UploadFileAsync(new MemoryStream(data), url, "Image", storageFolder: importedDir).Result;

            });
        }
        
        protected override Task<int> Execute(ImportPagesCmdletOptions args) =>
            _db.ExecAsync(async db =>
            {
                MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
                AppDbContextSeed.Seed(_db);

                using var t = db.BeginTransaction();
                var import = JsonConvert.DeserializeObject<ImporterServicePageDatabase>(
                    (await File.ReadAllTextAsync(args.Input)));

                var langIds = db.Languages.ToDictionary(x => x.LanguageShortName, x => x.Id);
                
                
                foreach (var page in import.Pages)
                {
                    Console.WriteLine("Importing " + string.Join(" / ", page.Languages.Values.Select(v => v.Title)));
                    int rootId = 0;
                    foreach (var lang in page.Languages)
                    {
                        var langId = langIds[lang.Key];
                        var found = db.Pages.FirstOrDefault(p => p.LanguageId == langId && p.Url == lang.Value.Url);
                        if (found != null)
                        {
                            rootId = found.PageRootId;
                            Console.WriteLine("Updating existing root " + rootId);
                            break;
                        }
                    }

                    if (rootId == 0)
                        rootId = db.InsertWithInt32Identity(new PageRoot());
                    db.Pages.Delete(p => p.PageRootId == rootId);

                    foreach (var lang in page.Languages)
                    {
                        var rows = new JArray();
                        void AppendRow(string block, JObject data)
                        {
                            rows.Add(new JObject
                            {
                                ["maxWidth"] = 1170,
                                ["blocks"] = new JArray()
                                {
                                    new JObject
                                    {
                                        ["size"] = 12,
                                        ["type"] = block,
                                        ["data"] = data
                                    }
                                }
                            });
                        }

                        if (!string.IsNullOrWhiteSpace(lang.Value.TitleImageUrl))
                            AppendRow("imageBlock", new JObject
                            {
                                ["align"] = "justify-center",
                                ["image"] = EnsureLegacyImageUrl(lang.Value.TitleImageUrl)
                            });
                        AppendRow("fixedHeightBlock", new JObject
                        {
                            ["text"] = lang.Value.CollapseBlock,
                            ["header"] = lang.Value.CollapseBlockTitle,
                            ["height"] = 200
                        });
                        foreach (var b in lang.Value.Blocks)
                            AppendRow("HtmlWithIconBlock", new JObject
                            {
                                ["title"] = b.Title,
                                ["html"] = b.Content,
                                ["icon"] = b.Icon,
                                ["link"] = b.Link ?? ""
                            });
                        
                        
                        
                        
                        db.Insert(new Page
                        {
                            LanguageId = langIds[lang.Key],
                            Title = lang.Value.Title,
                            Url = lang.Value.Url,
                            PageRootId = rootId,
                            PageData = new JObject
                            {
                                ["rows"] = rows
                            }.ToString()
                        });
                    }

                }
                
                t.Commit();
                return 0;
            });


    }
}