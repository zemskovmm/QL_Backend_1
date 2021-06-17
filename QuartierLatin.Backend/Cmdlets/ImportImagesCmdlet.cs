using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NickBuhro.Translit;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Database;
using QuartierLatin.Backend.Database.AppDbContextSeed;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.FolderModels;
using QuartierLatin.Backend.Utils;
using QuartierLatin.Importer;
using QuartierLatin.Importer.DataModel;

namespace QuartierLatin.Backend.Cmdlets
{
    public class ImportGalleryCmdlet : CmdletBase<ImportGalleryCmdlet.ImportGalleryOptions>
    {
        private readonly AppDbContextManager _db;
        private readonly IFileAppService _fileAppService;
        private readonly DatabaseConfig _dbConfig;

        [Verb("import-gallery")]
        public class ImportGalleryOptions
        {
            [Value(0)] public string Input { get; set; }
        }

        public ImportGalleryCmdlet(AppDbContextManager db, IConfiguration config, IFileAppService fileAppService)
        {
            _db = db;
            _fileAppService = fileAppService;
            _dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
        }

        int EnsureFolder(string name, int? parent) =>
            _db.Exec(db =>
            {
                var dir =
                    db.StorageFolders.Where(x => x.FolderParentId == parent && x.FolderName == name)
                        .Select(x => x.Id).FirstOrDefault();

                if (dir == 0)
                    dir = db.InsertWithInt32Identity(new StorageFolder
                    {
                        FolderName = name,
                        FolderParentId = parent
                    });
                return dir;
            });

        int EnsureFileWithName(string name, int parentId, Func<Stream> dataGetter) =>
            _db.Exec(db =>
            {
                var existing =
                    db.Blobs.FirstOrDefault(b => b.OriginalFileName == name && b.StorageFolderId == parentId);
                if (existing != null)
                    return existing.Id;
                using var data = dataGetter();
                return _fileAppService.UploadFileAsync(data, name, "Image", storageFolder: parentId).Result;
            });

        int EnsureImage(int uni, string subDir, string path)
        {
            return _db.Exec(db =>
            {
                var dir = EnsureFolder(uni.ToString(), EnsureFolder("<Universities>", null));
                if (subDir != null)
                    dir = EnsureFolder(subDir, dir);

                using var file = File.OpenRead(path);
                byte[] hash;
                using (var md5 = MD5.Create())
                    hash = md5.ComputeHash(file);
                file.Position = 0;
                var name = Path.GetFileNameWithoutExtension(path) + "_" +
                           BitConverter.ToString(hash).ToLowerInvariant().Replace("-", "") + "." +
                           Path.GetExtension(path);

                return EnsureFileWithName(name, dir, () => file);
            });
        }
        
        protected override Task<int> Execute(ImportGalleryOptions args) =>
            _db.ExecAsync(async db =>
            {
                MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
                foreach (var uniDir in Directory.GetDirectories(args.Input))
                {
                    var id = int.Parse(Path.GetFileName(uniDir).Split(' ')[0]);
                    var uni = db.Universities.FirstOrDefault(x => x.Id == id);
                    if (uni == null)
                    {
                        Console.Error.WriteLine($"University {id} not found");
                        continue;
                    }

                    var logoNames = new[] {"logo.png.png", "logo.png", "logo.jpg"}.Select(l => Path.Combine(uniDir, l));
                    var logoPath = logoNames.FirstOrDefault(File.Exists);
                    
                    var headerPath = Path.Combine(uniDir, "Header", "MAIN.jpg");
                    var galleryPath = Path.Combine(uniDir, "Gallery");
                    if (logoPath != null)
                        uni.LogoId = EnsureImage(id, null, logoPath);
                    else
                        Console.Error.WriteLine($"Logo for {id} not found");
                    //, found files: " +string.Join(", ", Directory.GetFiles(uniDir).Select(Path.GetFileName)));

                    if (File.Exists(headerPath))
                        uni.BannerId = EnsureImage(id, null, headerPath);
                    else
                        Console.Error.WriteLine($"Header/MAIN.jpg for {id} not found");

                    db.Update(uni);


                    if (!Directory.Exists(galleryPath))
                    {
                        Console.Error.WriteLine($"Gallery for {id} not found");
                        continue;
                    }

                    var files = new List<int>();
                    foreach (var galleryFile in Directory.GetFiles(galleryPath)
                        .OrderBy(x =>
                        {
                            if (int.TryParse(x.Split('.')[0], out var num))
                                return num;
                            return 99999;
                        }).ThenBy(x => x))
                    {
                        files.Add(EnsureImage(id, "gallery", galleryFile));
                    }

                    using (var t = db.BeginTransaction())
                    {
                        db.UniversityGalleries.Delete(x => x.UniversityId == id);
                        foreach (var f in files)
                            db.UniversityGalleries.Insert(() => new UniversityGallery {UniversityId = id, ImageId = f});
                        t.Commit();
                    }
                }
                return 0;
            });


    }
}