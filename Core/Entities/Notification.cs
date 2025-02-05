using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Notification
    {
        public int ID { get; set; }
        public NotificationType NotificationType { get; set; }

        public string Message { get; set; }
        public string ReceiverID { get; set; } // Admin ID
        public ApplicationUser Receiver { get; set; }
    }
}
