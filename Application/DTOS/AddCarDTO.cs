using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class AddCarDTO
    {
        public string OwnerID { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int GovernorateID { get; set; }
        public int City { get; set; }
        public List<IFormFile> Pictures { get; set; } // For receiving image files
        public List<string>? PictureUrls { get; set; } // For storing image URLs
    }
}
