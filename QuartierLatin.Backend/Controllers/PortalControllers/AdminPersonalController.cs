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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.PortalApplicationDto;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize]
    [Route("/api/admin/personal/applications")]
    public class AdminPersonalController : Controller
    {
        private readonly IChatAppService _chatAppService;
        private readonly IFileAppService _fileAppService;
        private readonly IPortalPersonalAppService _personalAppService;

        public AdminPersonalController(IChatAppService chatAppService, IFileAppService fileAppService,
            IPortalPersonalAppService portalPersonalAppService)
        {
            _chatAppService = chatAppService;
            _fileAppService = fileAppService;
            _personalAppService = portalPersonalAppService;
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

        [HttpGet(),
         ProducesResponseType(typeof(CatalogSearchResponseDtoList<PortalApplicationAdminDto>), 200),
         ProducesResponseType(404)]
        public async Task<IActionResult> GetApplicationCatalog([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] ApplicationType? type, 
            [FromQuery] ApplicationStatus? status, [FromQuery] bool? isAnswered, [FromQuery] string? firstName, [FromQuery] string? lastName,
            [FromQuery] string? email, [FromQuery] string? phone)
        {
            var portalApplicationList = await _personalAppService.GetApplicationCatalogAdminAsync(type, status, isAnswered, firstName, lastName, email, phone, null, page, pageSize);

            var portalDtos = portalApplicationList.portalApplications.Select(application => new PortalApplicationAdminDto
            {
                Id = application.application.Id,
                Type = application.application.Type,
                EntityId = application.application.EntityId,
                Status = application.application.Status,
                CommonApplicationInfo = JObject.Parse(application.application.CommonTypeSpecificApplicationInfo),
                EntityTypeSpecificApplicationInfo = JObject.Parse(application.application.EntityTypeSpecificApplicationInfo),
                FirstName = application.user.FirstName,
                LastName = application.user.LastName,
                UserId = application.user.Id
            }).ToList();

            var response = new CatalogSearchResponseDtoList<PortalApplicationAdminDto>
            {
                Items = portalDtos,
                TotalItems = portalApplicationList.totalItems,
                TotalPages = FilterHelper.PageCount(portalApplicationList.totalItems, pageSize)
            };

            return Ok(response);
        }

        [HttpGet("user/{userId}"),
         ProducesResponseType(typeof(CatalogSearchResponseDtoList<PortalApplicationAdminDto>), 200),
         ProducesResponseType(404)]
        public async Task<IActionResult> GetUserApplicationCatalog([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] ApplicationType? type,
            [FromQuery] ApplicationStatus? status, [FromQuery] bool? isAnswered, [FromQuery] string? firstName, [FromQuery] string? lastName,
            [FromQuery] string? email, [FromQuery] string? phone, int userId)
        {
            var portalApplicationList = await _personalAppService.GetApplicationCatalogAdminAsync(type, status, isAnswered, firstName, lastName, email, phone, userId, page, pageSize);

            var portalDtos = portalApplicationList.portalApplications.Select(application => new PortalApplicationAdminDto
            {
                Id = application.application.Id,
                Type = application.application.Type,
                EntityId = application.application.EntityId,
                Status = application.application.Status,
                CommonApplicationInfo = JObject.Parse(application.application.CommonTypeSpecificApplicationInfo),
                EntityTypeSpecificApplicationInfo = JObject.Parse(application.application.EntityTypeSpecificApplicationInfo),
                FirstName = application.user.FirstName,
                LastName = application.user.LastName,
                UserId = application.user.Id
            }).ToList();

            var response = new CatalogSearchResponseDtoList<PortalApplicationAdminDto>
            {
                Items = portalDtos,
                TotalItems = portalApplicationList.totalItems,
                TotalPages = FilterHelper.PageCount(portalApplicationList.totalItems, pageSize)
            };

            return Ok(response);
        }

        [HttpGet("{id}"),
         ProducesResponseType(typeof(PortalApplicationDto), 200),
         ProducesResponseType(404)]
        public async Task<IActionResult> GetApplicationCatalog(int id)
        {
            var application = await _personalAppService.GetApplicationAsync(id);

            if (application is null)
                return NotFound();

            var response = new PortalApplicationDto
            {
                Id = application.Id,
                Type = application.Type,
                EntityId = application.EntityId,
                Status = application.Status,
                CommonApplicationInfo = JObject.Parse(application.CommonTypeSpecificApplicationInfo),
                EntityTypeSpecificApplicationInfo = JObject.Parse(application.EntityTypeSpecificApplicationInfo)
            };

            return Ok(response);
        }

        [HttpPut("{id}"),
         ProducesResponseType(200),
         ProducesResponseType(403)]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody] PortalApplicationWithoutIdDto updatePortalApplication)
        {
            var response = await _personalAppService.UpdateApplicationAsync(id, updatePortalApplication.Type,
                updatePortalApplication.EntityId, updatePortalApplication.CommonApplicationInfo,
                updatePortalApplication.EntityTypeSpecificApplicationInfo);

            if (response is false)
                return Forbid();

            return Ok();
        }
    }
}
