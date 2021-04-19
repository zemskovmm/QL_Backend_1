using System;
using ClosedXML.Excel;

namespace QuartierLatin.Importer
{
    public class ImporterException : Exception
    {
        public ImporterException(string message, IXLWorksheet sheet, int row, int cell) : base(
            $"{message} at {sheet.Name} {sheet.Cell(row, cell).Address}")
        {
            
        }
        
        public ImporterException(string message) : base(message)
        {
            
        }
    }
}