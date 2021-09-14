using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.AppStateModels
{
    [Table("AppStateEntrys")]
    public class AppStateEntry
    {
        [PrimaryKey] public string Key { get; set; }
        [Column] public string Value { get; set; }
    }
}
