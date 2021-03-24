using System;

namespace QuartierLatin.Backend.Models.Cache
{
    public class GlobalSettingsCacheKey
    {
        public string Key { get; set; }
        public int LanguageId { get; set; }

        public GlobalSettingsCacheKey(string key, int languageId)
        {
            Key = key;
            LanguageId = languageId;
        }
        protected bool Equals(GlobalSettingsCacheKey other)
        {
            return Key == other.Key && LanguageId == other.LanguageId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GlobalSettingsCacheKey) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, LanguageId);
        }
    }
}
