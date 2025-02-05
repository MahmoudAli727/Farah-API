using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FavoriteService
    {
        public int ID { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
