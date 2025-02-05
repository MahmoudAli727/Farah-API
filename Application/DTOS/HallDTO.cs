using Application.Interfaces;
using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOS
{
    public class HallDTO : IServiceDTO
    {
        public int HallID { get; set; }
        public string OwnerID { get; set; }
        public int Price { get; set; }
        public List<string> Features { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public int GovernorateID { get; set; }
        public int City { get; set; }
        public List<IFormFile> Pictures { get; set; } // For receiving image files
        public List<string>? PictureUrls { get; set; } // For storing image URLs
        public ServiceStatus ServiceStatus { get; set; }

        public bool IsFavorite { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
