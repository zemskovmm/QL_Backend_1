using Microsoft.AspNetCore.Http;

namespace QuartierLatin.Backend.Dto.Media
{
    public class CreateMediaDto
    {
        public IFormFile UploadedFile { get; set; }

        public string FileType { get; set; }

        public int? StorageFolderId { get; set; }
    }
}
