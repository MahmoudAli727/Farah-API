namespace Application.DTOS
{
    public class ServiceForBeautyCenterDTO
    {
        public int ID { get; set; }
        public int BeautyCenterId { get; set; }
        public string Name { get; set; } // Makeup artist
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Appointment { get; set; }

    }
}
