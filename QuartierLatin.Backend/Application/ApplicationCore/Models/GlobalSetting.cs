using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    [Table("GlobalSettings")]
    public class GlobalSetting
    {
        [Column] [PrimaryKey] public string Key { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string JsonData { get; set; }
    }
}
