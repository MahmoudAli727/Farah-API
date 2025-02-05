namespace Core.Entities
{
    public class ServiceForBeautyCenter
    {
        public int Id { get; set; }
        public string Name { get; set; } // Makeup artist
        public string? Description { get; set; }
        public decimal? Price { get; set; } //  بكتب جمب كل سيرفس السعر بتاعها والسعر الكلي في جدول المواعيد
        public string Appointment { get; set; }
        public int BeautyCenterId { get; set; }
        public BeautyCenter BeautyCenter { get; set; }
    }
}
