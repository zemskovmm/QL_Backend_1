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
                    blocks = new object[]
                    {
                        new
                        {
                            type = "text",
                            data = "Lorem ipsum dolor sit amet"
                        },
                        new
                        {
                            type = "standard-service-block",
                            data = new
                            {
                                generalInformation = "Lorem ipsum",
                                advantages = new[] {"Lorem", "ipsum"},
                                requirements = new[] {"Lorem", "ipsum"},
                                prices = new[] {"Lorem", "ipsum"}
                            }
                        }
                    },
                }
            });
        }

        public static JObject GetCommonTrait(int? order = null)
        {
            return JObject.FromObject(new
            {
                names = new
                {
                    blocks = new object[]
                    {
                        new
                        {
                            type = "text",
                            data = "Lorem ipsum dolor sit amet"
                        },
                        new
                        {
                            type = "standard-service-block",
                            data = new
                            {
                                generalInformation = "Lorem ipsum",
                                advantages = new[] {"Lorem", "ipsum"},
                                requirements = new[] {"Lorem", "ipsum"},
                                prices = new[] {"Lorem", "ipsum"}
                            }
                        }
                    },
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
                    blocks = new object[]
                    {
                        new
                        {
                            type = "text",
                            data = "Lorem ipsum dolor sit amet"
                        },
                        new
                        {
                            type = "standard-service-block",
                            data = new
                            {
                                generalInformation = "Lorem ipsum",
                                advantages = new[] {"Lorem", "ipsum"},
                                requirements = new[] {"Lorem", "ipsum"},
                                prices = new[] {"Lorem", "ipsum"}
                            }
                        }
                    },
                },
                order = order ??= 1
            });
        }
    }
}
