﻿using System;

namespace QuartierLatin.Backend.Dto.PersonalChatDto
{
    public class PortalChatMessageListDto : PortalChatMessageDto
    {
        public string Author { get; set; }
        public int? BlobId { get; set; }
        public DateTime Date { get; set; }
    }
}