using Microsoft.AspNetCore.Http;

namespace Application.DTOS
{
    public class DressDto
    {
        public int Id { get; set; } // Ensure this property exists to identify dresses
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsForRent { get; set; }
        public bool IsSaled { get; set; }

        public List<IFormFile> Images { get; set; } // For receiving image files
        public List<string> ImageUrls { get; set; } = new List<string>(); // For storing image URLs
        public int ShopId { get; set; }
    }
}
