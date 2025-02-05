using Application.Interfaces;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOS
{
    public class PhotographyDTO : IServiceDTO
    {
        public int photographyID { get; set; }
        public List<ReviewsPhoto> Reviews { get; set; }
        public string OwnerID { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Pictures { get; set; } // For receiving image files
        public List<string>? PictureUrls { get; set; } // For storing image URLs
        public ServiceStatus ServiceStatus { get; set; }

        public bool IsFavorite { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
