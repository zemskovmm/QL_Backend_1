using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    public class GlobalSetting
    {
        [Column] public string Key { get; set; }
        [Column] public int LanguageId { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string JsonData { get; set; }
    }
}
