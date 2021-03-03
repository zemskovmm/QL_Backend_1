using QuartierLatin.Admin.Database;

namespace QuartierLatin.Admin.Config
{
    public class DatabaseConfig
    {
        public DatabaseType Type { get; set; }
        public string ConnectionString { get; set; }
    }
}