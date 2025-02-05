using Application.Interfaces;
using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOS
{
    public class CarDTO : IServiceDTO
    {
        //CarID
        public int CarID { get; set; }

        public bool IsFavorite { get; set; }
        public string OwnerID { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int GovernorateID { get; set; }
        public int City { get; set; }
        public List<IFormFile> Pictures { get; set; } // For receiving image files
        public List<string>? PictureUrls { get; set; } // For storing image URLs3
        public ServiceStatus ServiceStatus { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
