namespace Core.Entities
{
    public class ImagesBeautyCenter
    {
        public int Id { get; set; }
        public int BeautyCenterId { get; set; }
        public string ImageUrl { get; set; }
        public BeautyCenter beautyCenter { get; set; }


    }
}
