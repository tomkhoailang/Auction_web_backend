namespace Chat.Service.Models
{
    public class CreateMessageModel
    {
        public string? Content { get; set; }
        public int ChatRoomId;
        public string SenderId;

    }
}
