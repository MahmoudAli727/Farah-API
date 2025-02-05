namespace Core.Entities
{
    public class ReviewsPhoto
    {
        public int Id { get; set; }
        //public string UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } //من خمسه 
        public int PhotoId { get; set; }
        public Photography Photography { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
