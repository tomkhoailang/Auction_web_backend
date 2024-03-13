namespace Chat.Data.Models
{
    public class ChatRoomProduct
    {
        public int ChatRoomProductId { get; set; }
        public int ChatRoomId { get; set; }
        public int ProductId { get; set; }
        public DateTime? BiddingStartTime { get; set; }
        public DateTime? BiddingEndTime { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual ChatRoom ChatRoom { get; set; } = null!;
    }
}
