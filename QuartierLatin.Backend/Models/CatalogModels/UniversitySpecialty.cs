using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("UniversitySpecialties")]
    public class UniversitySpecialty
    {
        [PrimaryKey]
        public int UniversityId { get; set; }
        [PrimaryKey]
        public int SpecialtyId { get; set; }
        [Column]
        public int CostFrom { get; set; }
        [Column]
        public int CostTo { get; set; }
    }
}