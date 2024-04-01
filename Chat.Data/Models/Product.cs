using Chat.Data.Interfaces;

namespace Chat.Data.Models
{
    public class Product : ISoftDelete
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal InitialPrice { get; set; }
        public decimal MinimumStep { get; set; }
        public bool IsSold { get; set; }
        public string SellerId { get; set; } = null!;
        public virtual ApplicationUser Seller { get; set; } = null!;
        public virtual ICollection<ProductImage>? Images { get; set; }
        public virtual ICollection<Bidding>? Biddings { get; set; }
        public virtual ICollection<ProductInStatus>? ProductInStatuses { get; set; }
        public virtual ICollection<ChatRoomProduct>? ChatRoomProducts { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
