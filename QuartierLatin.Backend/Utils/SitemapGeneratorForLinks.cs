using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Database;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.AppStateRepository;
using X.Web.Sitemap;

namespace QuartierLatin.Backend.Utils
{
    public class SitemapGeneratorForLinks
    {
        private readonly AppDbContextManager _db;
        private readonly ISitemapGenerator _sitemapGenerator;
        private readonly ISitemapIndexGenerator _sitemapIndexGenerator;
        private readonly ILanguageRepository _languageRepository;
        private readonly IAppStateEntryRepository _appStateEntryRepository;
        private readonly string _domainName;
        private readonly string _directory;
        public SitemapGeneratorForLinks(AppDbContextManager db, ISitemapGenerator sitemapGenerator,
            ISitemapIndexGenerator sitemapIndexGenerator, ILanguageRepository languageRepository,
            IAppStateEntryRepository appStateEntryRepository, IOptions<SitemapConfig> config,
            IWebHostEnvironment env)
        {
            _sitemapGenerator = sitemapGenerator;
            _sitemapIndexGenerator = sitemapIndexGenerator;
            _db = db;
            _languageRepository = languageRepository;
            _appStateEntryRepository = appStateEntryRepository;
            _domainName = config.Value.DomainName;
            _directory = Path.Combine(env.ContentRootPath, config.Value.Directory);
        }

        public async void GenerateSitemaps()
        {
            var universityPageUrlStrings = await GetUniversityUrls();
            var coursePageUrlStrings = await GetCourseUrls();
            var schoolPageUrlStrings = await GetSchoolsUrls();
            var pageUrlStrings = await GetPagesUrls();

            var allUrls = new List<Url>();

            allUrls.AddRange(GetUrlFromStringList(universityPageUrlStrings));
            allUrls.AddRange(GetUrlFromStringList(coursePageUrlStrings));
            allUrls.AddRange(GetUrlFromStringList(schoolPageUrlStrings));
            allUrls.AddRange(GetUrlFromStringList(pageUrlStrings));

            var targetSitemapDirectory = new DirectoryInfo(_directory);

            if (!Directory.Exists(targetSitemapDirectory.FullName))
                Directory.CreateDirectory(targetSitemapDirectory.FullName);

            var fileInfoForGeneratedSitemaps = _sitemapGenerator.GenerateSitemaps(allUrls, targetSitemapDirectory);

            var sitemapInfos = new List<SitemapInfo>();
            var dateSitemapWasUpdated = DateTime.UtcNow.Date;

            foreach (var fileInfo in fileInfoForGeneratedSitemaps)
            {
                var uriToSitemap = new Uri(_domainName + $"sitemaps/{fileInfo.Name}");

                sitemapInfos.Add(new SitemapInfo(uriToSitemap, dateSitemapWasUpdated));
            }

            _sitemapIndexGenerator.GenerateSitemapIndex(sitemapInfos, targetSitemapDirectory, "sitemap-index.xml");

            await _appStateEntryRepository.UpdateValueAsync("LastUpdate", DateTime.Now.ToString());
        }

        private async Task<List<string>> GetUniversityUrls()
        {
            var language = await GetLanguages();

            return await _db.ExecAsync(db => db.UniversityLanguages.Select(university => _domainName + language[university.LanguageId] + "/university/" + university.Url).ToListAsync());
        }

        private async Task<List<string>> GetCourseUrls()
        {
            var language = await GetLanguages();

            return await _db.ExecAsync(db => db.CourseLanguages.Select(course => _domainName + language[course.LanguageId] + "/course/" + course.Url).ToListAsync());
        }

        private async Task<List<string>> GetSchoolsUrls()
        {
            var language = await GetLanguages();

            return await _db.ExecAsync(db => db.SchoolLanguages.Select(university => _domainName + language[university.LanguageId] + "/school/" + university.Url).ToListAsync());
        }

        private async Task<List<string>> GetPagesUrls()
        {
            var language = await GetLanguages();

            return await _db.ExecAsync(db => db.Pages.Select(page => _domainName + language[page.LanguageId] + "/" + page.Url).ToListAsync());
        }

        private async Task<Dictionary<int, string>> GetLanguages() =>
           await _languageRepository.GetLanguageIdWithShortNameAsync();

        private IEnumerable<Url> GetUrlFromStringList(List<string> urls) =>
            urls.Select(url => new Url
            {
                Location = url,
                ChangeFrequency = ChangeFrequency.Daily,
                TimeStamp = DateTime.UtcNow,
                Priority = .9
            });
    }
}
