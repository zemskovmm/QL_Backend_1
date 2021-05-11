using ClosedXML.Excel;

namespace QuartierLatin.Importer
{
    public static class Extensions
    {
        public static string GetString(this IXLWorksheet sheet, int row, int cell) =>
            sheet.Cell(row, cell).Value.ToString();

        public static int? GetNInt(this IXLWorksheet sheet, int row, int cell)
        {
            var v = sheet.Cell(row, cell).Value;
            if (v is int i)
                return i;
            var s = v?.ToString();
            if (s == null)
                return null;
            if (int.TryParse(s, out i))
                return i;
            return null;
        }

        public static int GetInt(this IXLWorksheet sheet, int row, int cell) => GetNInt(sheet, row, cell).Value;

        public static string IfSpace(this string s, string value) => string.IsNullOrWhiteSpace(s) ? value : s;
    }
}