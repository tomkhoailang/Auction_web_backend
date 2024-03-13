namespace Chat.Data.Models
{
    public class ProductInStatus
    {
        public int ProductInStatusID { get; set; }
        public DateTime Timestamp { get; set; }
        public int ProductId { get; set; }
        public int ProductStatusId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual ProductStatus ProductStatus { get; set; } = null!;


    }
}
