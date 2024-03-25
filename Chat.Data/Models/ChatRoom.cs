namespace Chat.Data.Models
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public string HostUserId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ApplicationUser HostUser { get; set; } = null!;
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<ApplicationUser>? Users { get; set; }
        public virtual ICollection<ChatRoomProduct>? ChatRoomProducts { get; set; }

    }
}
