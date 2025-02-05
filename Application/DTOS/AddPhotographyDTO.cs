using Microsoft.AspNetCore.Http;

namespace Application.DTOS
{
    public class AddPhotographyDTO
    {
        public string OwnerID { get; set; }

        public string Description { get; set; }
        public List<IFormFile> Pictures { get; set; } // For receiving image files
        public List<string>? PictureUrls { get; set; } // For storing image URLs
    }
}
