namespace Chat.Service.Models
{
    public class TokenType
    {
        public string Token { get; set; } = null!;
        public DateTime ExpirationTokenDate { get; set; }
    }
}
