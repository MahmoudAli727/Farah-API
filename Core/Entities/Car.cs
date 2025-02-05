using Application.Helpers;

namespace Core.Entities
{
    public class Car : Service
    {
        public string Brand { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public List<CarPicture> Pictures { get; set; }
        public int GovernorateID { get; set; }
        public int City { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
