using Chat.Data.Interfaces;

namespace Chat.Data.Models
{
    public class ChatRoom : ISoftDelete
    {
        public int ChatRoomId { get; set; }
        public string HostUserId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ApplicationUser HostUser { get; set; } = null!;
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<ChatRoomUser>? Users { get; set; }
        public virtual ICollection<ChatRoomProduct>? ChatRoomProducts { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
