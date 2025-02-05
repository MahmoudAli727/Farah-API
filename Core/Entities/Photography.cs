using Application.Helpers;

namespace Core.Entities
{
    public class Photography : Service
    {
        public string Description { get; set; }
        public List<ReviewsPhoto> Reviews { get; set; }
        public List<Portfolio> Images { get; set; }
    }
}
