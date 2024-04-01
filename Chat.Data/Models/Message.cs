using Chat.Data.Interfaces;

namespace Chat.Data.Models
{
    public class Message : ISoftDelete
    {
        public int MessageId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public int ChatRoomId { get; set; }
        public string SenderId { get; set; } = null!;
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual ApplicationUser Sender { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
