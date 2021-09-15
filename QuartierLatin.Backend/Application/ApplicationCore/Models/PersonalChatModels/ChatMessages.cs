using LinqToDB.Mapping;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.PersonalChatModels
{
    [Table("ChatMessages")]
    public class ChatMessages : BaseModel
    {
        [Column] public MessageType MessageType { get; set; }
        [Column] public string Author { get; set; }
        [Column] public string Text { get; set; }
        [Column] public int? BlobId { get; set; }
        [Column] public int ChatId { get; set; }
    }
}
