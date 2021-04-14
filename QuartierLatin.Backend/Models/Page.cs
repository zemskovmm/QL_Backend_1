﻿using LinqToDB;
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
    }
}