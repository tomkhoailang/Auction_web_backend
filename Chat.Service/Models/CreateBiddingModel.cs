namespace Chat.Service.Models
{
    public class CreateBiddingModel
    {
        public decimal BiddingAmount { get; set; }
        public string? BiddingUserId { get; set; }
        public int ProductId { get; set; }
    }
}
