using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    public abstract class BaseModel
    {
        [PrimaryKey, Identity] public virtual int Id { get; set; }
    }
}
