using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Database;
using QuartierLatin.Backend.Models.Repositories;
using X.Web.Sitemap;

namespace QuartierLatin.Backend.Utils
{
    public class SitemapGeneratorForLinks
    {
        private readonly AppDbContextManager _db;
        private readonly ISitemapGenerator _sitemapGenerator;
        private readonly ISitemapIndexGenerator _sitemapIndexGenerator;
        private readonly ILanguageRepository _languageRepository;
        private const string _domainName = "https://quartier-latin.com/";
        public SitemapGeneratorForLinks(AppDbContextManager db, ISitemapGenerator sitemapGenerator,
            ISitemapIndexGenerator sitemapIndexGenerator, ILanguageRepository languageRepository)
        {
            _sitemapGenerator = sitemapGenerator;
            _sitemapIndexGenerator = sitemapIndexGenerator;
            _db = db;
            _languageRepository = languageRepository;
        }

        public async void GenerateSitemaps()
        {
            var universityPageUrlStrings = await GetUniversityUrls();
            var coursePageUrlStrings = await GetCourseUrls();
            var schoolPageUrlStrings = await GetSchoolsUrls();

            var allUrls = new List<Url>();

            allUrls.AddRange(GetUrlFromStringList(universityPageUrlStrings));
            allUrls.AddRange(GetUrlFromStringList(coursePageUrlStrings));
            allUrls.AddRange(GetUrlFromStringList(schoolPageUrlStrings));

            var targetSitemapDirectory = new DirectoryInfo("\\sitemaps\\");

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

            return await _db.ExecAsync(db => db.UniversityLanguages.Select(university => _domainName + language[university.LanguageId] + "/school/" + university.Url).ToListAsync());
        }

        private async Task<Dictionary<int, string>> GetLanguages() =>
           await _languageRepository.GetLanguageIdWithShortNameAsync();

        private IEnumerable<Url> GetUrlFromStringList(List<string> urls) =>
            urls.Select(url => new Url
            {
                Location = url,
                ChangeFrequency = ChangeFrequency.Monthly,
                TimeStamp = DateTime.UtcNow,
                Priority = .9
            });
    }
}
