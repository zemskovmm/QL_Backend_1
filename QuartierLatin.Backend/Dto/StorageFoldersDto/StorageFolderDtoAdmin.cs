using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.StorageFoldersDto
{
    public class StorageFolderDtoAdmin : BaseDto
    {
        public string FolderName { get; set; }

        public List<BlobItemDtoAdmin> Files { get; set; }
    }
}
