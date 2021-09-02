﻿using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Portal;

namespace QuartierLatin.Backend.Models.Repositories.PortalRepository
{
    public interface IPortalPersonalRepository
    {
        Task<int> CreateApplicationAsync(ApplicationType type, int entityId, JObject applicationInfo, JObject entityTypeSpecificApplicationInfo);
        Task UpdateApplicationAsync(int id, ApplicationType type, int entityId, JObject? applicationInfo, JObject? entityTypeSpecificApplicationInfo);
        Task<PortalApplication> GetApplicationAsync(int id);
    }
}