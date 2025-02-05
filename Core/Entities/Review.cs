namespace Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } //من خمسه 
        public int BeautyCenterId { get; set; }
        public BeautyCenter BeautyCenter { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
