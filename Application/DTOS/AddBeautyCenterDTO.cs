using Microsoft.AspNetCore.Http;

namespace Application.DTOS
{
    public class AddBeautyCenterDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Gove { get; set; }
        public string OwnerID { get; set; }
        public int City { get; set; }
        public List<IFormFile> Images { get; set; } // For receiving image files
        public List<string>? ImageUrls { get; set; } // For storing image URLs
        //public ServiceForBeautyCenterDTO Services { get; set; }


    }
}
