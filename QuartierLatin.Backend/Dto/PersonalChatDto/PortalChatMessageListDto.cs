using System;

namespace QuartierLatin.Backend.Dto.PersonalChatDto
{
    public class PortalChatMessageListDto : PortalChatMessageDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public int? BlobId { get; set; }
        public DateTime Date { get; set; }
    }
}
