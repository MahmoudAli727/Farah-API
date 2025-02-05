using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class HallPicture
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public int HallID { get; set; }
        public Hall Hall { get; set; }
    }
}
