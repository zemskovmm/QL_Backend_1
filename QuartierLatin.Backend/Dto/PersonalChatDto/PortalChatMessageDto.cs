using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Dto.PersonalChatDto
{
    public class PortalChatMessageDto
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
    }
}
