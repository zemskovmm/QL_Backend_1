using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels
{
    [Table("Chats")]
    public class Chat : BaseModel
    {
        public int PortalUserId { get; set; }
    }
}
