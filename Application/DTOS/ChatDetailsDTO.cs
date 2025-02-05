using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class ChatDetailsDTO
    {
        public int ChatId { get; set; }
        public UserDTO User { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
