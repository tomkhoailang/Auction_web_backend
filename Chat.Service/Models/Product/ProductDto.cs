using Chat.Data.Models;

namespace Chat.Service.Models.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal InitialPrice { get; set; }
        public decimal MinimumStep { get; set; }
        public bool IsSold { get; set; }
        public string SellerId { get; set; } = null!;
        public virtual ApplicationUser Seller { get; set; } = null!;
        public ICollection<ProductImageDto>? Images { get; set; }
        public ICollection<Bidding>? Biddings { get; set; }
        public ICollection<ProductInStatus>? ProductInStatuses { get; set; }
        public ICollection<ChatRoomProduct>? ChatRoomProducts { get; set; }
    }
}
