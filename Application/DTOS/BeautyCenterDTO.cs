using Application.Interfaces;
using Core.Enums;

namespace Application.DTOS
{
    public class BeautyCenterDTO : IServiceDTO
    {
        public int BeautyCenterId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Gove { get; set; }
        public string OwnerID { get; set; }
        public int City { get; set; }
        // public List<IFormFile> Images { get; set; } // For receiving image files
        public List<string>? ImageUrls { get; set; } // For storing image URLs
        public List<ServiceForBeautyCenterDTO> Services { get; set; }
        public List<ReviewForBeautyCenterDTO> Reviews { get; set; }
        public ServiceStatus ServiceStatus { get; set; }

        public bool IsFavorite { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
