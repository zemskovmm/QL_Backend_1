using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models.AppStateModels
{
    [Table("AppStateEntrys")]
    public class AppStateEntry
    {
        [PrimaryKey] public string Key { get; set; }
        [Column] public string Value { get; set; }
    }
}
