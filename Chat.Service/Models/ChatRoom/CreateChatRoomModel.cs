namespace Chat.Service.Models.ChatRoom
{
    public class CreateChatRoomModel
    {
        public string? HostUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
