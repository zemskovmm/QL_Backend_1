using System.Collections.Generic;

namespace QuartierLatin.Admin.Dto
{
    public class ListResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}