using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.CatalogModels
{
    [Table("Specialties")]
    public class Specialty : BaseNamedModel
    {
        [System.ComponentModel.DataAnnotations.Schema.Column]
        public int CategoryId { get; set; }
    }
}