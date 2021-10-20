using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/application/files")]
    public class AdminPortalApplicationFileStorageController : Controller
    {
        private readonly IPortalApplicationFileStorageAppService _portalApplicationFileStorageAppService;

        public AdminPortalApplicationFileStorageController(IPortalApplicationFileStorageAppService portalApplicationFileStorageAppService)
        {
            _portalApplicationFileStorageAppService = portalApplicationFileStorageAppService;
        }

        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetFilesToApplication(int applicationId)
        {
            var response = await _portalApplicationFileStorageAppService.GetFilesToApplicationAsync(applicationId);
            return Ok(response);
        }

        [HttpPost("{applicationId}/{blobId}")]
        public async Task<IActionResult> CreateFileItemToApplication(int applicationId, int blobId)
        {
            await _portalApplicationFileStorageAppService.CreateFileItemToApplicationAsync(applicationId, blobId);
            return Ok(new object());
        }

        [HttpDelete("{applicationId}/{blobId}")]
        public async Task<IActionResult> DeleteFileItemToApplication(int applicationId, int blobId)
        {
            await _portalApplicationFileStorageAppService.DeleteFileItemToApplicationAsync(applicationId, blobId);
            return Ok(new object());
        }
    }
}
