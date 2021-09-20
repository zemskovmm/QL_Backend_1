using System.IO;

namespace QuartierLatin.Backend.Utils
{
    public static class FileUtils
    {
        public static bool CheckFileType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            return ext.ToLower() switch
            {
                ".gif" => true,
                ".jpg" => true,
                ".jpeg" => true,
                ".png" => true,
                _ => false
            };
        }
    }
}
