using Chat.Data.Data;
using Chat.Data.Models;
using Chat.Service.Models;
using Chat.Service.Models.ChatRoom;
using Microsoft.EntityFrameworkCore;

namespace Chat.Service.Services
{
    public class ChatRoomManagement : IChatRoomManagement
    {
        private readonly ApplicationDbContext _dbcontext;

        public ChatRoomManagement(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<ApiResponse<ChatRoom>> GetChatRoomAsync(int ChatRoomId)
        {
            ChatRoom chatRoom = await _dbcontext.ChatRooms.Include(cr => cr.ChatRoomProducts).Include(cr => cr.Users).FirstOrDefaultAsync(cr => cr.ChatRoomId == ChatRoomId);
            if (chatRoom == null)
            {
                return new ApiResponse<ChatRoom> { IsSuccess = false, Message = "No chatroom found with that id", StatusCode = 404 };
            }
            return new ApiResponse<ChatRoom> { IsSuccess = true, Message = "Chat room is found", StatusCode = 200, Response = chatRoom };

        }
        public async Task<ApiResponse<ChatRoom>> CreateChatRoomAsync(CreateChatRoomModel createChatRoomModel)
        {
            ChatRoom chatRoom = new ChatRoom
            {
                HostUserId = createChatRoomModel.HostUserId,
                StartDate = createChatRoomModel.StartDate,
                EndDate = createChatRoomModel.EndDate,
            };
            await _dbcontext.ChatRooms.AddAsync(chatRoom);
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<ChatRoom> { IsSuccess = true, Message = "Create chat room sucessully", StatusCode = 201, Response = chatRoom };
            }
            return new ApiResponse<ChatRoom> { IsSuccess = false, Message = "Create chat room failed", StatusCode = 400 };
        }

        public Task<ApiResponse<List<ChatRoom>>> GetUserChatRooms(ApplicationUser user)
        {
            user.JoinedChatRooms ??= new List<ChatRoom>();
            List<ChatRoom> chatRooms = user.JoinedChatRooms.ToList();
            var filteredChatRooms = chatRooms.Select(room => new ChatRoom
            {
                ChatRoomId = room.ChatRoomId,
                HostUserId = room.HostUserId,
                StartDate = room.StartDate,
                EndDate = room.EndDate,
                Messages = room.Messages,
                Users = room.Users!.Select(u => new ApplicationUser { UserName = u.UserName }).ToList()
            }).ToList();
            return Task.FromResult(new ApiResponse<List<ChatRoom>> { IsSuccess = false, Message = "Find chatroom succesfully", StatusCode = 200, Response = filteredChatRooms });
        }

        public async Task<ApiResponse<Message>> CreateMessageAsync(CreateMessageModel createMessageModel)
        {
            Message message = new Message()
            {
                ChatRoomId = createMessageModel.ChatRoomId,
                Content = createMessageModel.Content,
                SenderId = createMessageModel.SenderId,
                Timestamp = DateTime.UtcNow,
            };
            await _dbcontext.Messages.AddAsync(message);
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Message> { IsSuccess = true, Message = "Create message sucessully", StatusCode = 201, Response = message };
            }
            return new ApiResponse<Message> { IsSuccess = false, Message = "Create message failed", StatusCode = 400 };
        }
    }
}
