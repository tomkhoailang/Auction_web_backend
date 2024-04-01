using Microsoft.AspNetCore.Identity;

namespace Chat.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Avatar { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<ChatRoomUser>? JoinedChatRooms { get; set; }
        public ICollection<ChatRoom>? HostRooms { get; set; }
        public ICollection<Product>? SellingProducts { get; set; }
        public ICollection<Bidding>? Biddings { get; set; }

    }
}
