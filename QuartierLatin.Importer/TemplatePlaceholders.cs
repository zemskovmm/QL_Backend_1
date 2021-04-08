using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Bibliography;
using QuartierLatin.Importer.DataModel;

namespace QuartierLatin.Importer
{
    public class TemplatePlaceholders
    {
        public static void InitializeWithPlaceholders(ImporterDatabase db)
        {
            int nextSpecialtyCatId = 1;
            int nextSpecialtyId = 1;
            int nextCityId = 1;
            db.Specialties = new List<ImporterSpecialtyCategory>
            {
                new ImporterSpecialtyCategory
                {
                    Id = nextSpecialtyCatId++,
                    Names =
                    {
                        ["ru"] = "Право",
                        ["en"] = "Ley",
                        ["fr"] = "Loi",
                        ["esp"] = "Ley"
                    },
                    Specialties = new List<ImporterSpecialty>
                    {
                        new ImporterSpecialty
                        {
                            Id = nextSpecialtyId++,
                            Names =
                            {
                                ["ru"] = "Гражданское право",
                                ["en"] = "Civil law",
                                ["fr"] = "Loi civille",
                                ["esp"] = "Ley civil"
                            }
                        },
                        new ImporterSpecialty
                        {
                            Id = nextSpecialtyId++,
                            Names =
                            {
                                ["ru"] = "Уголовное право",
                                ["en"] = "Criminal law",
                                ["fr"] = "Loi criminelle ",
                                ["esp"] = "Derecho penal"
                            }
                        }
                    }
                },
                new ImporterSpecialtyCategory
                {
                    Id = nextSpecialtyCatId++,
                    Names =
                    {
                        ["ru"] = "Финансы",
                        ["en"] = "Finance",
                        ["fr"] = "la finance",
                        ["esp"] = "Finanzas "
                    },
                    Specialties = new List<ImporterSpecialty>
                    {
                        new ImporterSpecialty
                        {
                            Id = nextSpecialtyId++,
                            Names =
                            {
                                ["ru"] = "Бухучет",
                                ["en"] = "Accounting",
                                ["fr"] = "Comptabilité ",
                                ["esp"] = "Contabilidad"
                            }
                        }
                    }
                }
            };

            db.Cities = new List<ImporterCity>
            {
                new ImporterCity
                {
                    Id = nextCityId++,
                    Names =
                    {
                        ["en"] = "Paris",
                        ["fr"] = "Paris",
                        ["ru"] = "Париж",
                        ["esp"] = "París"
                    }
                },
                new ImporterCity
                {
                    Id = nextCityId++,
                    Names =
                    {
                        ["en"] = "Rouen",
                        ["fr"] = "Rouen",
                        ["ru"] = "Руан",
                        ["esp"] = "Ruan"
                    }
                }

            };
            
            var unis = db.Universities.OrderBy(x => x.Id).ToList();
            unis[0].Website = "http://example.com";
            unis[0].FoundationYear = 1745;
            unis[0].MinumumAge = 17;
            unis[0].LanguagesOfInstruction = new List<string> {"en", "fr"};
            unis[0].Cities.AddRange(new[] {1, 2});
            
            
            unis[0].Specialties.Add(new ImporterUniversitySpecialtyMapping
            {
                Cost = 10000,
                SpecialtyId = 1
            });
            unis[0].Specialties.Add(new ImporterUniversitySpecialtyMapping
            {
                Cost = 20000,
                SpecialtyId = 2
            });
            
            
            unis[1].LanguagesOfInstruction = new List<string> {"fr"};
            unis[1].Cities.Add(2);
            unis[1].Specialties.Add(new ImporterUniversitySpecialtyMapping
            {
                Cost = 5000,
                SpecialtyId = 3
            });
            
            
        }
    }
}