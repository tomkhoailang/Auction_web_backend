using Chat.Data.Interfaces;

namespace Chat.Data.Models
{
    public class ProductImage : ISoftDelete
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
