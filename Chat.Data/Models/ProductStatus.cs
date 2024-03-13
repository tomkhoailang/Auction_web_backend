namespace Chat.Data.Models
{
    public class ProductStatus
    {
        public int ProductStatusId { get; set; }
        public string ProductStatusName { get; set; } = null!;
        public virtual ICollection<ProductInStatus>? ProductInStatuses { get; set; }
    }
}
