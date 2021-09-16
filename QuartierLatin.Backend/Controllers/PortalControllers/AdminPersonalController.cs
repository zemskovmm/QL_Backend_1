using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Dto.PersonalChatDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize]
    [Route("/api/admin/personal/applications")]
    public class AdminPersonalController : Controller
    {
        private readonly IChatAppService _chatAppService;

        public AdminPersonalController(IChatAppService chatAppService)
        {
            _chatAppService = chatAppService;
        }

        [HttpGet("{id}/chat/messages"),
         ProducesResponseType(typeof(List<PortalChatMessageListDto>), 200),
         ProducesResponseType(404)]
        public async Task<IActionResult> GetChatMessages(int id)
        {
            var userId = GetUserId();

            var messages = await _chatAppService.GetChatMessagesAsync(id, userId);

            if (messages is null || messages.Count is 0)
                return NotFound();

            var response = messages.Select(message => new PortalChatMessageListDto
            {
                Author = message.Author,
                BlobId = message.BlobId,
                Text = message.Text,
                Type = message.MessageType
            });

            return Ok(response);
        }

        [HttpPost("{id}/chat/messages"),
         ProducesResponseType(200)]
        public async Task<IActionResult> SendChatMessages(int id, [FromBody] PortalChatMessageDto messageDto)
        {
            var userId = GetUserId();

            var response = await _chatAppService.SendChatMessageAsync(id, userId, messageDto.Text, messageDto.Type);

            if (response is false)
                return BadRequest();

            return Ok();
        }

        [HttpGet("/chats"),
         ProducesResponseType(typeof(List<ChatDto>), 200),
         ProducesResponseType(404)]
        public async Task<IActionResult> GetChats()
        {
            var response = await _chatAppService.GetChatsAsync();

            return Ok(response);
        }

        private int GetUserId()
        {
            var userClaims = User.Identities.FirstOrDefault(identity =>
                identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme).Claims;

            var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);
            return userId;
        }
    }
}
