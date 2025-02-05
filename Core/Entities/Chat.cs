namespace Core.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public List<ChatMessage>? Messages { get; set; }

        public string OwnerId { get; set; }
        public Owner Owner { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
