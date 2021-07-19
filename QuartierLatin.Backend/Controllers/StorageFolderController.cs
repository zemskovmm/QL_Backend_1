﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.StorageFolders;
using QuartierLatin.Backend.Dto.StorageFoldersDto;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/media/directories")]
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
            var childFolder = await _storageFolderAppService.GetChildFoldersAsync(storageFolder.Id);
            var filesInFolder = await _storageFolderAppService.GetFilesInfoInFolderAsync(storageFolder.Id);

            var response = new StorageFolderDtoAdmin
            {
                Id = storageFolder.Id,
                FolderName = storageFolder.FolderName,
                ParentId = storageFolder.FolderParentId,
                Files = filesInFolder.Select(file =>
                        new BlobItemDtoAdmin
                        {
                            Id = file.Id,
                            OriginalFileName = file.OriginalFileName
                        }).ToList(),
                Directories = childFolder.Select(directory => new DirectoryDtoAdmin
                {
                    DirectoryName = directory.FolderName,
                    Id = directory.Id
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet()]
        public async Task<IActionResult> GetDefaultStorageFolder()
        {
            var filesInFolder = await _storageFolderAppService.GetFilesInfoInDefaultFolderAsync();
            var childFolder = await _storageFolderAppService.GetDefaultChildFoldersAsync();

            var response = new StorageFolderDtoAdmin
            {
                Files = filesInFolder.Select(file =>
                    new BlobItemDtoAdmin
                    {
                        Id = file.Id,
                        OriginalFileName = file.OriginalFileName
                    }).ToList(),
                Directories = childFolder.Select(directory => new DirectoryDtoAdmin
                {
                    DirectoryName = directory.FolderName,
                    Id = directory.Id
                }).ToList()

            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStorageFolder(int id)
        {
            await _storageFolderAppService.RemoveStorageFolderAsync(id);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStorageFolder(int id, [FromBody] PatchStorageFolderDtoAdmin patchDto)
        {
            await _storageFolderAppService.UpdateFolderNameAsync(id, patchDto.StorageFolderName);

            return Ok();
        }
    }
}
