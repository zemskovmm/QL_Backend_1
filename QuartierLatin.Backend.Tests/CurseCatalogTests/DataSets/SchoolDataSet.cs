using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Tests.CurseCatalogTests.DataSets
{
    public static class SchoolDataSet
    {
        public static JObject GetSchool(int? foundationYear = null)
        {
            return JObject.FromObject(new
            {
                foundationYear = foundationYear ?? 1990,
                languages = new
                {
                    ru = new
                    {
                        name = "тест",
                        htmlDescription = "Видишь тест. И я его не вижу, а он есть",
                        url = "/тест/123"
                    },
                    en = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = "/test/123"
                    },
                }
            });
        }

        public static JObject GetSchoolWithAdditionalLanguage(int? foundationYear = null)
        {
            return JObject.FromObject(new
            {
                foundationYear = foundationYear ?? 1990,
                languages = new
                {
                    ru = new
                    {
                        name = "тест",
                        htmlDescription = "Видишь тест. И я его не вижу, а он есть",
                        url = "/тест/123"
                    },
                    en = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = "/test/123"
                    },
                    fr = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = "/test/123"
                    },
                }
            });
        }
    }
}
