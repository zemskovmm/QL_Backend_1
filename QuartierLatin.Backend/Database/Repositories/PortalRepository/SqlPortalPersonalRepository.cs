using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Portal;
using QuartierLatin.Backend.Models.Repositories.PortalRepository;

namespace QuartierLatin.Backend.Database.Repositories.PortalRepository
{
    public class SqlPortalPersonalRepository : IPortalPersonalRepository
    {
        private readonly AppDbContextManager _db;

        public SqlPortalPersonalRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateApplicationAsync(ApplicationType type, int entityId, JObject applicationInfo,
            JObject entityTypeSpecificApplicationInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateApplicationAsync(int id, ApplicationType type, int entityId, JObject? applicationInfo,
            JObject? entityTypeSpecificApplicationInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PortalApplication> GetApplicationAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
