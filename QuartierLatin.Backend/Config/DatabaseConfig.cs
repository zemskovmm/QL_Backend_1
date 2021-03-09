using QuartierLatin.Backend.Database;

namespace QuartierLatin.Backend.Config
{
    public class DatabaseConfig
    {
        public DatabaseType Type { get; set; }
        public string ConnectionString { get; set; }
    }
}