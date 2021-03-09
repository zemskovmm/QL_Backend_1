using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto
{
    public class ListResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}