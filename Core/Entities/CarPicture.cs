using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CarPicture
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public int CarID { get; set; }
        public Car Car { get; set; }
    }
}
