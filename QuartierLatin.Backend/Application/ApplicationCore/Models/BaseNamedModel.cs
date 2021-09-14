using System.Collections.Generic;
using LinqToDB;
using LinqToDB.Mapping;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    public abstract class BaseNamedModel : BaseModel
    {
        private JsonbHolder<Dictionary<string, string>> _names = new();
        [Column("Names", DataType = DataType.BinaryJson)]
        public string NamesJson
        {
            get => _names.Json;
            set => _names.Json = value;
        }
        
        public Dictionary<string, string> Names
        {
            get => _names.Value;
            set => _names.Value = value;
        }
    }
}