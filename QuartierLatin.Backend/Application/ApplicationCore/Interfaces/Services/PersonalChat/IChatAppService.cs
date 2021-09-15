using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat
{
    public interface IChatAppService
    {
        Task JoinUserToChatAsync(int userId, string roomName, string connectionId);
        Task JoinAdminToChatAsync(int userId, string roomName, string connectionId);
    }
}
