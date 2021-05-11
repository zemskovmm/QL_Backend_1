using System;
using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel
{
    public class ImporterDatabase
    {
        public List<ImporterUniversity> Universities { get; set; } = new();
        public List<ImporterCity> Cities { get; set; } = new();
        public List<ImporterAccreditation> Accreditations { get; set; } = new();
        public List<ImporterCertification> Certifications { get; set; } = new();
        public List<ImporterCountry> Countries { get; set; } = new();
        public List<ImporterSpecialtyCategory> Specialties { get; set; } = new();
    }
}