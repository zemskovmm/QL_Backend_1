using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CommandLine;
using HtmlAgilityPack;
using Newtonsoft.Json;
using QuartierLatin.Importer.DataModel;

namespace QuartierLatin.Importer
{
    [Verb("parse-html", HelpText = "Parse HTML into university database JSON")]
    // ReSharper disable once UnusedType.Global
    class ParseHtmlCommand: ICommandLineCommand
    {
        [Option('i', "input", Required = true)] public string InputDirectory { get; set; }
        [Option('o', "output", Required = true)] public string OutputFile { get; set; }

        public int Execute()
        {
            var db = new ImporterDatabase();
            var dic = new Dictionary<int, ImporterUniversity>();
            foreach (var langDir in Directory.GetDirectories(InputDirectory))
            {
                var lang = Path.GetFileName(langDir);
                foreach (var file in Directory.GetFiles(langDir))
                {
                    var id = int.Parse(Path.GetFileName(file).Split(' ')[0]);
                    if (!dic.TryGetValue(id, out var uni))
                        db.Universities.Add(dic[id] = uni = new ImporterUniversity {Id = id});

                    uni.Languages[lang] = ParseLanguage(file);
                }
            }


            File.WriteAllText(OutputFile, JsonConvert.SerializeObject(db, Formatting.Indented));
            return 0;
        }




        ImporterUniversityLanguage ParseLanguage(string pathToHtml)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(File.ReadAllText(pathToHtml));
            
            var body = doc.DocumentNode.SelectSingleNode("//body");

            string name = null;
            foreach (var n in body.ChildNodes)
            {
                if (n.Name == "p")
                {
                    name = HttpUtility
                        .HtmlDecode(n.InnerText.Replace("&nbsp;", " "))
                        .Trim().Replace("\n", " ");
                    while (name.Contains("  "))
                        name = name.Replace("  ", " ");
                    n.Remove();
                    break;
                }
            }

            if (name == null)
                throw new Exception("Invalid data in " + pathToHtml);

            var firstTable = body.ChildNodes.FirstOrDefault(x => x.Name.ToLowerInvariant() == "table");
            if (firstTable != null)
            {
                while (true)
                {
                    var ch = body.LastChild;
                    ch.Remove();
                    if (ch == firstTable)
                        break;
                }
            }

            foreach (var d in body.Descendants())
            {
                foreach(var a in d.Attributes.ToList())
                    if (a.Name.ToLowerInvariant() == "style" || a.Name == "lang")
                        a.Remove();
            }
            
            body.Descendants().Where(x =>
                x.Name.ToLowerInvariant() == "table"
                || x.NodeType == HtmlNodeType.Comment
            ).ToList().ForEach(e => e.Remove());

            foreach (var a in body.Descendants().Where(x => x.Name.ToLowerInvariant() == "a").ToList())
            {
                if(a.InnerText.Trim().Length==0)
                    a.Remove();
                else
                {
                    a.Name = "span";
                    a.Attributes.RemoveAll();
                }
            }

            foreach (var li in body.Descendants().Where(x => x.Name.ToLowerInvariant() == "li").ToList())
            {
                while (li.NextSibling.NodeType == HtmlNodeType.Text && li.NextSibling.InnerText.Trim().Length == 0)
                    li.NextSibling.Remove();
                if (li.NextSibling.Name == "p")
                {
                    var p = li.NextSibling;
                    p.Remove();
                    li.ChildNodes.Add(p);
                }
            }

            foreach (var p in body.Descendants().Where(x => x.Name.ToLowerInvariant() == "p").ToList())
            {
                if(p.InnerText.Trim().Length == 0)
                    p.Remove();
            }

            return new ImporterUniversityLanguage
            {
                Name = name,
                HtmlData = body.InnerHtml.Trim()
            };
        }
    }
}