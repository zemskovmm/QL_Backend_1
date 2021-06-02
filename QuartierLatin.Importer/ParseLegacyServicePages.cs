using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using CommandLine;
using HtmlAgilityPack;
using Newtonsoft.Json;
using QuartierLatin.Importer.DataModel;

namespace QuartierLatin.Importer
{
    
    [Verb("parse-legacy-services", HelpText = "Parse HTML into pages database JSON")]
    public class ParseLegacyServicePages : ICommandLineCommand
    {
        static readonly string[] Langs = new[] {"ru", "fr", "en", "cn"};
        
        [Option('i', "input", Required = true)] public string InputList { get; set; }
        [Option('o', "output", Required = true)] public string OutputFile { get; set; }
        [Option('c', "cache", Required = true)] public string CacheDirectory { get; set; }

        HtmlAgilityPack.HtmlDocument GetDocumentFromUrl(string url)
        {
            Directory.CreateDirectory(CacheDirectory);
            string hash;
            using (var sha = SHA1.Create())
                hash = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(url)));
            var cacheFile = Path.Combine(CacheDirectory, hash);
            if (!File.Exists(cacheFile))
                new WebClient().DownloadFile(url, cacheFile);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(cacheFile);
            return doc;
        }

        string GetHtml(HtmlNode node)
        {
            foreach (var d in node.Descendants().ToList())
            {
                if (d.NodeType == HtmlNodeType.Comment || d.Name.ToLowerInvariant() == "img")
                {
                    d.Remove();
                    continue;
                }

                if(d.NodeType == HtmlNodeType.Text)
                    continue;

                var classesAttrs = d.GetAttributes().Where(x => x.Name.ToLowerInvariant() == "class")
                    .ToList();
                if (classesAttrs.Count != 0)
                {
                    var classes = string.Join(" ", d.GetAttributeValue("class", "").Split(' ',
                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(c => "legacy-web-" + c));
                    classesAttrs.ForEach(x => x.Remove());
                    if (classes.Length != 0)
                        d.SetAttributeValue("class", classes);
                }
            }

            return node.InnerHtml;
        }
        
        ImporterServicePageLanguage ParseLanguage(string url)
        {
            var uri = new Uri(url);
            var path = uri.AbsolutePath.TrimStart('/');
            if (Langs.Any(path.StartsWith))
                path = path.Substring(3).TrimStart('/');

            var doc = GetDocumentFromUrl(url);
            var title = doc.DocumentNode.SelectSingleNode("//head/title").InnerText.Trim();
            var containerDiv = doc.DocumentNode.SelectSingleNode("//body/div[contains(@class, 'remodal-bg')]");
            var containerElements = containerDiv.ChildNodes.Where(c => c.NodeType == HtmlNodeType.Element).ToList();

            var headerDiv = containerElements[2];
            var imageDiv = headerDiv.SelectSingleNode("//div[contains(@class, 'category-latest-post-article')]");
            var imageDivStyle = imageDiv.GetAttributeValue("style", "");
            var imageRegexResult = Regex.Match(imageDivStyle, "background-image: url\\('([^']+)").Groups[1].Value;
            if (string.IsNullOrWhiteSpace(imageRegexResult))
                throw new Exception("No image for " + url);

            var contentDiv = containerElements[3].ChildNodes.First(n => n.NodeType == HtmlNodeType.Element);

            var page = new ImporterServicePageLanguage
            {
                Title = title,
                TitleImageUrl = imageRegexResult,
                Url = path
            };
                
            if (contentDiv.Descendants().Any(d => d.GetAttributeValue("class", "").Contains("article_icon")))
            {
                var elementChildNodes = contentDiv.ChildNodes.Where(n => n.NodeType == HtmlNodeType.Element).ToList();
                var header = elementChildNodes[0];
                var content = elementChildNodes[1];
                header.Remove();
                content.Remove();
                page.CollapseBlockTitle = header.InnerText.Trim();
                page.CollapseBlock = GetHtml(content);
            }

            page.MainBlock = GetHtml(contentDiv);

            return page;
        }
        
        public int Execute()
        {
            var db = new ImporterServicePageDatabase();
            foreach (var line in File.ReadAllLines(InputList))
            {
                var page = new ImporterServicePage();
                var sp = line.Split(';');
                for (var c = 0; c < sp.Length; c++)
                {
                    var url = sp[c];
                    if (!string.IsNullOrWhiteSpace(url))
                        page.Languages[Langs[c]] = ParseLanguage(url);
                }

                db.Pages.Add(page);
            }
            File.WriteAllText(OutputFile, JsonConvert.SerializeObject(db, Formatting.Indented));
            return 0;
        }
    }
}