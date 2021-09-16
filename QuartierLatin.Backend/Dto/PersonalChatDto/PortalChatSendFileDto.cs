using Microsoft.AspNetCore.Http;

namespace QuartierLatin.Backend.Dto.PersonalChatDto
{
    public class PortalChatSendFileDto
    {
        public IFormFile UploadedFile { get; set; }
    }
}
