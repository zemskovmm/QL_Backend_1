using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet
{
    public static class TraitDataSet
    {
        public static JObject GetTraitType(string? identifier = null)
        {
            return JObject.FromObject(new
            {
                identifier = identifier ??= "test",
                names = new
                {
                    ru = "тест",
                    en = "тест"
                }
            });
        }

        public static JObject GetCommonTrait(int? order = null)
        {
            return JObject.FromObject(new
            {
                names = new
                {
                    ru = "тест",
                    en = "тест"
                },
                order = order ??= 1
            });
        }

        public static JObject GetCommonTraitWithTraitTypeId(int? order = null, int? traitTypeId = null)
        {
            return JObject.FromObject(new
            {
                traitTypeId = traitTypeId ??= 0,
                names = new
                {
                    ru = "тест",
                    en = "тест"
                },
                order = order ??= 1
            });
        }
    }
}
