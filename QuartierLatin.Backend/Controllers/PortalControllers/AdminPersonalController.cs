using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Dto.PersonalChatDto;
using QuartierLatin.Backend.Utils;
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
            var messages = await _chatAppService.GetChatMessagesAdminAsync(id);

            if (messages is null || messages.Count is 0)
                return NotFound();

            var response = messages.Select(message => new PortalChatMessageListDto
            {
                Author = message.Author,
                BlobId = message.BlobId,
                Text = message.Text,
                Type = message.MessageType,
                Date = message.Date
            });

            return Ok(response);
        }

        [HttpPost("{id}/chat/messages"),
         ProducesResponseType(200)]
        public async Task<IActionResult> SendChatMessages(int id, [FromBody] PortalChatMessageDto messageDto)
        {
            var response = await _chatAppService.SendChatMessageAdminAsync(id, messageDto.Type, messageDto.Text);

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
            if (!FileUtils.CheckFileType(mediaDto.UploadedFile.FileName))
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
    }
}
