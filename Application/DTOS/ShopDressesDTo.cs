using Application.Interfaces;
using Core.Enums;

namespace Application.DTOS
{
    public class ShopDressesDTo : IServiceDTO
    {
        public int ShopDressesID { get; set; }
        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public string OwnerID { get; set; }
        public int GovernorateID { get; set; }
        public int City { get; set; }
        public DateTime OpeningHours { get; set; }
        public List<DressDto> Dresses { get; set; } = new List<DressDto>();
        public ServiceStatus ServiceStatus { get; set; }
    }
}
