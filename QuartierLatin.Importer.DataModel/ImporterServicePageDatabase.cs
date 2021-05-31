using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel
{
    public class ImporterServicePageDatabase
    {
        public List<ImporterServicePage> Pages { get; set; } = new();
    }

    public class ImporterServicePage
    {
        public Dictionary<string, ImporterServicePageLanguage> Languages { get; set; } = new();
    }

    public class ImporterServicePageBlock
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
    
    public class ImporterServicePageLanguage
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string TitleImageUrl { get; set; }
        public string CollapseBlockTitle { get; set; }
        public string CollapseBlock { get; set; }
        public string MainBlock { get; set; }
    }
}