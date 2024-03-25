using Chat.Data.Models;
using Chat.Service.Models;
using Chat.Service.Models.ChatRoom;

namespace Chat.Service.Services
{
    public interface IChatRoomManagement
    {
        public Task<ApiResponse<ChatRoom>> CreateChatRoomAsync(CreateChatRoomModel createChatRoomModel);
        public Task<ApiResponse<ChatRoom>> GetChatRoomAsync(int ChatRoomId);
        public Task<ApiResponse<Message>> DeleteChatRoomAsync(int ChatRoomId);
        public Task<ApiResponse<List<ChatRoom>>> GetUserChatRooms(ApplicationUser user);
        public Task<ApiResponse<List<ChatRoom>>> GetAllChatRooms();
        public Task<ApiResponse<Message>> CreateMessageAsync(CreateMessageModel createMessageModel);
        public Task<ApiResponse<ChatRoom>> EditChatRoomAsync(CreateChatRoomModel createChatRoomModel, int chatRoomId);
        public Task<ApiResponse<ChatRoom>> JoinChatRoom(int ChatRoomId, string UserId);


    }
}
