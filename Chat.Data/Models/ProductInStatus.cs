using Chat.Data.Interfaces;

namespace Chat.Data.Models
{
    public class ProductInStatus : ISoftDelete
    {
        public int ProductInStatusID { get; set; }
        public DateTime Timestamp { get; set; }
        public int ProductId { get; set; }
        public int ProductStatusId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual ProductStatus ProductStatus { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
