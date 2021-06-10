using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Tests.CurseCatalogTests.DataSets
{
    public static class CurseDataSet
    {
        public static JObject GetCurse(int schoolEntityId)
        {
            return JObject.FromObject(new
            {
                schoolId = schoolEntityId,
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

        public static JObject GetCurseWithAdditionalLanguage(int schoolEntityId)
        {
            return JObject.FromObject(new
            {
                schoolId = schoolEntityId,
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
