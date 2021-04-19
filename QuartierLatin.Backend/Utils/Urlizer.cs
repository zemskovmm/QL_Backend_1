using System.Text;
using NickBuhro.Translit;

namespace QuartierLatin.Backend.Utils
{
    public class Urlizer
    {
        public static string Urlize(string name)
        {
            name = Transliteration.CyrillicToLatin(name.ToLowerInvariant(), Language.Russian).ToLowerInvariant();

            StringBuilder sb = new StringBuilder();
            var arrayText = name.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                char? toAppend = null;
                if (
                    (letter >= 'a' && letter <= 'z')
                    || (letter >= 'A' && letter <= 'Z')
                    || (letter >= '0' && letter <= '9')
                    || letter == '-'
                )
                    toAppend = letter;
                else if (letter == ' ')
                    toAppend = '-';
                
                if (toAppend == '-'
                    && (sb.Length == 0 || sb[sb.Length - 1] == '-'))
                    toAppend = null;
                
                if (toAppend.HasValue)
                    sb.Append(toAppend.Value);
            }

            return sb.ToString().ToLowerInvariant();
        }
    }
}