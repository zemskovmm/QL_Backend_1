using System;
using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    [Table("Pages")]
    public class Page
    {
        [Column] public string Url { get; set; }
        [Column] public string Title { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string PageData { get; set; }
        [Column] [PrimaryKey] public int LanguageId { get; set; }
        [Column] [PrimaryKey] public int PageRootId { get; set; }
        [Column] public int? PreviewImageId { get; set; }
        [Column] public int? SmallPreviewImageId { get; set; }
        [Column] public int? WidePreviewImageId { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string? Metadata { get; set; }
        [Column] public DateTime? Date { get; set; }
    }
}
