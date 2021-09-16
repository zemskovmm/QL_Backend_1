using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Dto.PersonalChatDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize]
    [Route("/api/admin/personal/applications")]
    public class AdminPersonalController : Controller
    {
        private readonly IChatAppService _chatAppService;
        private readonly IFileAppService _fileAppService;

        public AdminPersonalController(IChatAppService chatAppService, IFileAppService fileAppService)
        {
            _chatAppService = chatAppService;
            _fileAppService = fileAppService;
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

            var response = await _chatAppService.SendChatMessageAsync(id, userId, messageDto.Type, messageDto.Text);

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

        [HttpPost("{id}/chat/messages/upload"),
         ProducesResponseType(200)]
        public async Task<IActionResult> SendChatMessages(int id, [FromBody] PortalChatSendFileDto mediaDto)
        {
            if (!CheckFileType(mediaDto.UploadedFile.FileName))
                return BadRequest();

            var provider = new FileExtensionContentTypeProvider();

            provider.TryGetContentType(mediaDto.UploadedFile.FileName, out var contentType);

            var blobId = await _fileAppService.UploadChatMediaAsync(mediaDto.UploadedFile.OpenReadStream(),
                mediaDto.UploadedFile.FileName, contentType);

            var response = await _chatAppService.SendChatMessageAdminAsync(id, MessageType.File, blobId: blobId);

            if (response is false)
                return BadRequest();

            return Ok();
        }

        private int GetUserId()
        {
            var userClaims = User.Identities.FirstOrDefault(identity =>
                identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme).Claims;

            var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);
            return userId;
        }
        private static bool CheckFileType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            return ext.ToLower() switch
            {
                ".gif" => true,
                ".jpg" => true,
                ".jpeg" => true,
                ".png" => true,
                _ => false
            };
        }
    }
}
