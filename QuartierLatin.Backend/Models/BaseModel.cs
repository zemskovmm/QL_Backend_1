using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    public abstract class BaseModel
    {
        [PrimaryKey, Identity] public virtual int Id { get; set; }
    }
}
