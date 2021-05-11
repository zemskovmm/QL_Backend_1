namespace QuartierLatin.Backend.Utils
{
    public static class FilterHelper
    {
        public static int PageCount(int resultCount, int pageSize) =>
            resultCount / pageSize + (resultCount % pageSize == 0 ? 0 : 1);
    }
}
