namespace Core.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }

        public string ImageURL { get; set; }

        public int PhotographerId { get; set; }

        public Photography Photographer { get; set; }

    }
}
