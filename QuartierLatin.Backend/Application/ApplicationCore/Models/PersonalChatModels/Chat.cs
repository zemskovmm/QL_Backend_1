using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels
{
    [Table("Chats")]
    public class Chat : BaseModel
    {
        [Column] public int PortalUserId { get; set; }
        [Column] public int ApplicationId { get; set; }
    }
}
