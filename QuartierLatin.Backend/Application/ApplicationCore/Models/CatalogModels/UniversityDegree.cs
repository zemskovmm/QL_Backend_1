using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels
{
    [Table("Degrees")]
    public class Degree : BaseNamedModel
    {
        
    }

    [Table("UniversityDegrees")]
    public class UniversityDegree
    {
        [Column]
        public int UniversityId { get; set; }
        [Column]
        public int DegreeId { get; set; }
        [Column]
        public int CostGroup { get; set; }
    }
}