using Newtonsoft.Json;

namespace QuartierLatin.Backend.Utils
{
    public class JsonbHolder<T> where T : class, new()
    {
        private readonly bool _autoCreate;

        public JsonbHolder(bool autoCreate = false)
        {
            _autoCreate = autoCreate;
        }

        private T _value;

        public string Json
        {
            get => _value == null ? null : JsonConvert.SerializeObject(_value);
            set => _value =
                value == null ? null : JsonConvert.DeserializeObject<T>(value);
        }

        public T Value
        {
            get => _value ?? (_autoCreate ? (_value = new T()) : null);
            set => _value = value;
        }
    }
}
