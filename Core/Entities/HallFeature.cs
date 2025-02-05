using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class HallFeature
    {
        public int Id { get; set; }
        public string Feature { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }
    }
}
