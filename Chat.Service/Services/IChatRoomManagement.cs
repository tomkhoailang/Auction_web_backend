using Chat.Data.Models;
using Chat.Service.Models;
using Chat.Service.Models.ChatRoom;

namespace Chat.Service.Services
{
    public interface IChatRoomManagement
    {
        public Task<ApiResponse<ChatRoom>> CreateChatRoomAsync(CreateChatRoomModel createChatRoomModel);
        public Task<ApiResponse<ChatRoom>> GetChatRoomAsync(int ChatRoomId);
        public Task<ApiResponse<List<ChatRoom>>> GetUserChatRooms(ApplicationUser user);
        public Task<ApiResponse<Message>> CreateMessageAsync(CreateMessageModel createMessageModel);




    }
}
