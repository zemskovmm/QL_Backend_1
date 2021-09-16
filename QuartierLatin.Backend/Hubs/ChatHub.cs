using Microsoft.AspNetCore.SignalR;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Constants;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;

namespace QuartierLatin.Backend.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatAppService _chatAppService;

        public ChatHub(IChatAppService chatAppService)
        {
            _chatAppService = chatAppService;
        }

        public async Task Join(int applicationId)
        {
            try
            {
                var userClaims = Context.User?.Identities?.FirstOrDefault(identity =>
                    identity?.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme)?.Claims;

                if (userClaims is null)
                {
                    userClaims = Context.User?.Identities?.FirstOrDefault(identity =>
                        identity?.AuthenticationType == CookieAuthenticationPortal.AuthenticationScheme)?.Claims;

                    if (userClaims is null)
                        throw new UnauthorizedAccessException();

                    var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);

                    await _chatAppService.JoinUserToChatAsync(userId, applicationId, Context.ConnectionId);
                }
                else
                {
                    var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);
                    await _chatAppService.JoinAdminToChatAsync(userId, applicationId, Context.ConnectionId);
                }


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        //TODO: Remove before merge
        public async Task JoinTest(int applicationId, int userId, bool admin)
        {
            try
            {
                if (admin is true)
                {
                    await _chatAppService.JoinAdminToChatAsync(userId, applicationId, Context.ConnectionId);
                    
                }
                else
                {
                    await _chatAppService.JoinUserToChatAsync(userId, applicationId, Context.ConnectionId);
                }


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(int applicationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, applicationId.ToString());
        }
    }
}
