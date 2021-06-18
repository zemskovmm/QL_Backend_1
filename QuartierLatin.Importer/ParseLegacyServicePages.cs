using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using CommandLine;
using DocumentFormat.OpenXml.Drawing.Diagrams;
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

        string GetText(HtmlNode node) => Regex.Replace(HttpUtility.HtmlDecode(node.InnerText).Trim(), @"\s+", " ");
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
                if (d.NodeType == HtmlNodeType.Element)
                {
                    foreach(var a in d.Attributes.ToList())
                        if (a.Name.ToLowerInvariant() != "href")
                            a.Remove();
                }
            }

            return node.OuterHtml;
        }
        
        ImporterServicePageLanguage ParseLanguage(string url)
        {
            var uri = new Uri(url);
            var path = uri.AbsolutePath.TrimStart('/');
            if (Langs.Any(path.StartsWith))
                path = path.Substring(3).TrimStart('/');

            var doc = GetDocumentFromUrl(url);
            var title = GetText(doc.DocumentNode.SelectSingleNode("//head/title"));
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
                Url = path,
                Blocks = new List<ImporterServicePageBlock>()
            };
                
            if (contentDiv.Descendants().Any(d => d.GetAttributeValue("class", "").Contains("article_icon")))
            {
                var header = contentDiv.ChildNodes.FirstOrDefault(n => n.NodeType == HtmlNodeType.Element);
                if (header.Name == "h2")
                {
                    page.CollapseBlockTitle = GetText(header);
                    header.Remove();
                }
            }

            ImporterServicePageBlock currentBlock = null;
            foreach (var ch in contentDiv.ChildNodes)
            {
                if (ch.NodeType != HtmlNodeType.Element)
                    continue;
                if (ch.Name.ToLowerInvariant() == "h2"
                    && ch.Descendants().FirstOrDefault(x => x.Name.ToLowerInvariant() == "img") is { } img
                    && img.GetAttributeValue("src", "").Contains("/uploads/other/article"))
                {
                    var src = img.GetAttributeValue("src", "");
                    var icon = src.Contains("article1") ? ImporterServicePageBlock.IconMedal
                        : src.Contains("article2") ? ImporterServicePageBlock.IconList
                        : ImporterServicePageBlock.IconLabel;

                    var a = ch.Descendants().FirstOrDefault(x => x.Name == "a");
                    string link = null;
                    if (a != null)
                    {
                        link = a.GetAttributeValue("href", "");

                        if (!link.ToLowerInvariant().StartsWith("http"))
                            link = "https://quartier-latin.com" + link;
                    }

                    currentBlock = new ImporterServicePageBlock
                    {
                        Title = GetText(ch),
                        Icon = icon,
                        Link = link
                    };
                    page.Blocks.Add(currentBlock);
                }
                else if (currentBlock != null)
                    currentBlock.Content += GetHtml(ch);
                else
                    page.CollapseBlock += GetHtml(ch);
            }

            if (page.Blocks.Count < 2)
                throw new Exception("Unrecognized page format");
            
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