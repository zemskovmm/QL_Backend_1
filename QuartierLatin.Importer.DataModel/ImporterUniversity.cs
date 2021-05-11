using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel
{
    public class ImporterUniversity
    {
        public int Id { get; set; }
        public Dictionary<string, ImporterUniversityLanguage> Languages { get; set; } = new();
        public int? FoundationYear { get; set; } 
        public string Url { get; set; }
        public List<string> LanguagesOfInstruction { get; set; } = new();
        public Dictionary<ImporterUniversityDegree, int> Degrees { get; set; } = new();
        public List<int> Cities { get; set; } = new();
        public List<int> Countries { get; set; } = new();
        public List<int> Specialties { get; set; } = new();
        public List<int> Accreditations { get; set; } = new();
        public List<int> Certifications { get; set; } = new();
    }
    

    public class ImporterNamedEntityBase
    {
        public int Id { get; set; }
        public Dictionary<string, string> Names { get; set; } = new();
    }

    public class ImporterCountry : ImporterNamedEntityBase
    {
        
    }
    
    public class ImporterCity : ImporterNamedEntityBase
    {
        
    }
    
    public class ImporterAccreditation :ImporterNamedEntityBase
    {
        
    }
    
    public class ImporterCertification :ImporterNamedEntityBase
    {
        
    }
    
    public class ImporterSpecialty : ImporterNamedEntityBase
    {
    }

}