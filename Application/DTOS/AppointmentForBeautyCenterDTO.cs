namespace Application.DTOS
{
    public class AppointmentForBeautyCenterDTO
    {
        public DateTime AppointmentDate { get; set; }
        // public string UserId { get; set; }
        public int BeautyCenterId { get; set; }

        public List<ServiceForBeautyCenterDTO> Services { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
