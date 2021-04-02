using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Tests.UniversityTest.UniversityTestsDataSet
{
    public static class UniversityDataSet
    {
        public static JObject GetUniversity(string? website = null, int? foundationYear = null)
        {
            return JObject.FromObject(new
            {
                website = "/test",
                foundationYear = 1999,
                languages = new
                {
                    ru = new
                    {
                        name = "тест",
                        htmlDescription = "Видишь тест. И я его не вижу, а он есть",
                        url = ""
                    },
                    en = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = ""
                    },
                }
            });
        }

        public static JObject GetUniversityWithChinaLang(string? website = null, int? foundationYear = null)
        {
            return JObject.FromObject(new
            {
                website = "/test",
                foundationYear = 1998,
                languages = new
                {
                    ru = new
                    {
                        name = "тест",
                        htmlDescription = "Видишь тест. И я его не вижу, а он есть",
                        url = ""
                    },
                    en = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = ""
                    },
                    cn = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = ""
                    },
                }
            });
        }
    }
}
