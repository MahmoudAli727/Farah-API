using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class MessageDTO
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string? Message { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
