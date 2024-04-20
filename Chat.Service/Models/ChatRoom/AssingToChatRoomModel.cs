namespace Chat.Service.Models.ChatRoom
{
    public class AssingToChatRoomModel
    {
        public List<int> ProductIds { get; set; } = null!;
        public int ChatRoomId { get; set; }
        public int Duration { get; set; }

    }
}
