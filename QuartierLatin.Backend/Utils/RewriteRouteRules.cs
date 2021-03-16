namespace QuartierLatin.Backend.Utils
{
    public static class RewriteRouteRules
    {
        public static string ReWriteRequests(string url)
        {
            return url.Replace("%2F", "/");
        }
    }
}
