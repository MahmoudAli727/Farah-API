namespace Core.Entities
{
    public class Dress
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsForRent { get; set; }
        public bool IsSaled { get; set; }
        public string ImageUrl { get; set; }
        public int ShopId { get; set; }
        public ShopDresses Shop { get; set; }
    }
}
