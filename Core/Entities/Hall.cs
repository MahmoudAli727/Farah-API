using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Hall : Service
    {
        public int Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public int GovernorateID { get; set; }
        public int City { get; set; }
        public List<HallPicture> Pictures { get; set; }

        public List<HallFeature> Features { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
