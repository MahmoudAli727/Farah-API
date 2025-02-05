using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class AllChatsDTO
    {
        public int chatId { get; set; }
        public UserDTO User { get; set; }
        public  string? lastMessage { get; set; }
        public DateTime? lastMessageSentAt { get; set; }
        public bool isRead { get; set; }
        public bool IamTheLastMessageSender { get; set; }

    }
}
