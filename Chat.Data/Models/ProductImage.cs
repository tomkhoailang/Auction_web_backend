namespace Chat.Data.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

    }
}
