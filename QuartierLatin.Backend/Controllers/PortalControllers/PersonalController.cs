using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PersonalChat;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Constants;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.PersonalChatDto;
using QuartierLatin.Backend.Dto.PortalApplicationDto;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
    [Route("/api/personal/applications")]
    public class PersonalController : Controller
    {
        private readonly IPortalPersonalAppService _personalAppService;
        private readonly IChatAppService _chatAppService;
        private readonly IFileAppService _fileAppService;

        public PersonalController(IPortalPersonalAppService personalAppService, IChatAppService chatAppService,
            IFileAppService fileAppService)
        {
            _personalAppService = personalAppService;
            _chatAppService = chatAppService;
            _fileAppService = fileAppService;
        }

        [HttpPost(),
         ProducesResponseType(200)]
        public async Task<IActionResult> CreateApplication([FromBody] PortalApplicationWithoutIdDto createApplication)
        {
            var userId = GetUserId();

            var id = await _personalAppService.CreateApplicationAsync(createApplication.Type, createApplication.EntityId,
                createApplication.CommonApplicationInfo, createApplication.EntityTypeSpecificApplicationInfo, userId);

            return Ok(new { id = id });
        }

        [HttpPost("{id}"),
         ProducesResponseType(200),
         ProducesResponseType(403)]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody]PortalApplicationWithoutIdDto updatePortalApplication)
        {
            var userId = GetUserId();

            var response = await _personalAppService.UpdateApplicationAsync(id, userId, updatePortalApplication.Type,
                updatePortalApplication.EntityId, updatePortalApplication.CommonApplicationInfo,
                updatePortalApplication.EntityTypeSpecificApplicationInfo);

            if (response is false)
                return Forbid();

            return Ok();
        }

        [HttpGet("{id}"),
         ProducesResponseType(typeof(PortalApplicationDto), 200),
         ProducesResponseType( 404)]
        public async Task<IActionResult> GetApplication(int id)
        {
            var userId = GetUserId();

            var application = await _personalAppService.GetApplicationAsync(id, userId);

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

        [HttpGet(),
         ProducesResponseType(typeof(CatalogSearchResponseDtoList<PortalApplicationDto>), 200)]
        public async Task<IActionResult> GetApplicationCatalog([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] ApplicationType? type, [FromQuery] ApplicationStatus? status)
        {
            var userId = GetUserId();

            var portalApplicationList = await _personalAppService.GetApplicationCatalogAsync(userId, type, status, page, pageSize);

            var portalDtos = portalApplicationList.portalApplications.Select(application => new PortalApplicationDto
            {
                Id = application.Id,
                Type = application.Type,
                EntityId = application.EntityId,
                Status = application.Status,
                CommonApplicationInfo = JObject.Parse(application.CommonTypeSpecificApplicationInfo),
                EntityTypeSpecificApplicationInfo = JObject.Parse(application.EntityTypeSpecificApplicationInfo)
            }).ToList();

            var response = new CatalogSearchResponseDtoList<PortalApplicationDto>
            {
                Items = portalDtos,
                TotalItems = portalApplicationList.totalItems,
                TotalPages = FilterHelper.PageCount(portalApplicationList.totalItems, pageSize)
            };

            return Ok(response);
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
                Type = message.MessageType,
                Date = message.Date
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

        [HttpPost("{id}/chat/messages/upload"),
         ProducesResponseType(200)]
        public async Task<IActionResult> SendChatMessages(int id, [FromForm] PortalChatSendFileDto mediaDto)
        {
            var userId = GetUserId();

            if (!FileUtils.CheckFileType(mediaDto.UploadedFile.FileName))
                return BadRequest();

            var provider = new FileExtensionContentTypeProvider();

            provider.TryGetContentType(mediaDto.UploadedFile.FileName, out var contentType);

            var blobId = await _fileAppService.UploadChatMediaAsync(mediaDto.UploadedFile.OpenReadStream(),
                mediaDto.UploadedFile.FileName, contentType);

            var response = await _chatAppService.SendChatMessageAsync(id, userId, MessageType.File, blobId:blobId);

            if (response is false)
                return BadRequest();

            return Ok();
        }

        private int GetUserId()
        {
            var userClaims = User.Identities.FirstOrDefault(identity =>
                identity.AuthenticationType == CookieAuthenticationPortal.AuthenticationScheme).Claims;

            var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);
            return userId;
        }
    }
}
