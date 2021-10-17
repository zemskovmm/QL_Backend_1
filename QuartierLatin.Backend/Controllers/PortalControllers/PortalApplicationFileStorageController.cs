using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Constants;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
    [Route("/api/application/files")]
    public class PortalApplicationFileStorageController : Controller
    {
        private readonly IPortalApplicationFileStorageAppService _portalApplicationFileStorageAppService;
        private readonly IPortalPersonalAppService _personalAppService;
        public PortalApplicationFileStorageController(IPortalApplicationFileStorageAppService portalApplicationFileStorageAppService, IPortalPersonalAppService portalPersonalAppService)
        {
            _portalApplicationFileStorageAppService = portalApplicationFileStorageAppService;
            _personalAppService = portalPersonalAppService;
        }

        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetFilesToApplication(int applicationId)
        {
            var userId = GetUserId();

            var isOwner = await _personalAppService.CheckIsUserOwnerAsync(userId, applicationId);

            if (isOwner is false)
                return Forbid();

            var response = await _portalApplicationFileStorageAppService.GetFilesToApplicationAsync(applicationId);
            return Ok(response);
        }

        [HttpPost("{applicationId}/{blobId}")]
        public async Task<IActionResult> CreateFileItemToApplication(int applicationId, int blobId)
        {
            var userId = GetUserId();

            var isOwner = await _personalAppService.CheckIsUserOwnerAsync(userId, applicationId);

            if (isOwner is false)
                return Forbid();

            await _portalApplicationFileStorageAppService.CreateFileItemToApplicationAsync(applicationId, blobId);
            return Ok(new object());
        }

        [HttpDelete("{applicationId}/{blobId}")]
        public async Task<IActionResult> DeleteFileItemToApplication(int applicationId, int blobId)
        {
            var userId = GetUserId();

            var isOwner = await _personalAppService.CheckIsUserOwnerAsync(userId, applicationId);

            if (isOwner is false)
                return Forbid();

            await _portalApplicationFileStorageAppService.DeleteFileItemToApplicationAsync(applicationId, blobId);
            return Ok(new object());
        }

        private int GetUserId()
        {
            var userClaims = User?.Identities?.FirstOrDefault(identity =>
                identity.AuthenticationType == CookieAuthenticationPortal.AuthenticationScheme)?.Claims;

            if (userClaims is null)
                throw new ArgumentNullException("User is null");

            var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);
            return userId;
        }
    }
}
