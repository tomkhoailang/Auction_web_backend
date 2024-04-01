using Chat.Data.Interfaces;

namespace Chat.Data.Models
{
    public class ChatRoomUser : ISoftDelete
    {
        public string UserId { get; set; } = null!;
        public int ChatRoomId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
