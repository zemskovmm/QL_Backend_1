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
        public int Cost { get; set; }
    }
}