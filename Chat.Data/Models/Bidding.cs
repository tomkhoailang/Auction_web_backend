namespace Chat.Data.Models
{
    public class Bidding
    {
        public int BiddingId { get; set; }
        public decimal BiddingAmount { get; set; }
        public DateTime Timestamp { get; set; }
        public string BiddingUserId { get; set; } = null!;
        public int ProductId { get; set; }
        public virtual ApplicationUser BiddingUser { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

    }
}
