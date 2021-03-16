﻿using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    [Table("Pages")]
    public class Page : BaseModel
    {
        [Column] public string Url { get; set; }
        [Column] public string Title { get; set; }
        [Column] public int LanguageId { get; set; }
        [Column] public int PageRootId { get; set; }
    }
}
