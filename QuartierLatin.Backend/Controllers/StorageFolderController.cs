using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.StorageFolders;
using QuartierLatin.Backend.Dto.StorageFoldersDto;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/folders")]
    public class StorageFolderController : Controller
    {
        private readonly IStorageFolderAppService _storageFolderAppService;
        public StorageFolderController(IStorageFolderAppService storageFolderAppService, IBlobRepository blobRepository)
        {
            _storageFolderAppService = storageFolderAppService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateStorageFolder([FromBody] CreateStorageFolderDtoAdmin createStorageFolderDtoAdmin)
        {
            var id = await _storageFolderAppService.CreateStorageFolderAsync(createStorageFolderDtoAdmin.FolderName,
                createStorageFolderDtoAdmin.FolderParentId);
            return Ok(new {id = id});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStorageFolder(int id)
        {
            var storageFolder = await _storageFolderAppService.GetStorageFolderByIdAsync(id);

            var filesInFolder = await _storageFolderAppService.GetFilesInfoInFolderAsync(storageFolder.Id);

            var response = new StorageFolderDtoAdmin
            {
                FolderName = storageFolder.FolderName,
                Files = filesInFolder.Select(file =>
                        new BlobItemDtoAdmin
                        {
                            FileType = file.FileType,
                            Id = file.Id,
                            OriginalFileName = file.OriginalFileName
                        }).ToList()
            };

            return Ok(response);
        }
    }
}
