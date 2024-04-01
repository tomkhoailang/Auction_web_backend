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
                EndDate = createChatRoomModel.StartDate.AddMinutes(5),
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
            //user.JoinedChatRooms ??= new List<ChatRoom>();
            //List<ChatRoom> chatRooms = user.JoinedChatRooms.ToList();
            //var filteredChatRooms = chatRooms.Select(room => new ChatRoom
            //{
            //    ChatRoomId = room.ChatRoomId,
            //    HostUserId = room.HostUserId,
            //    StartDate = room.StartDate,
            //    EndDate = room.EndDate,
            //    Messages = room.Messages,
            //    Users = room.Users!.Select(u => new ApplicationUser { UserName = u.UserName }).ToList()
            //}).ToList();
            //return Task.FromResult(new ApiResponse<List<ChatRoom>> { IsSuccess = false, Message = "Find chatroom succesfully", StatusCode = 200, Response = filteredChatRooms });
            throw new NotImplementedException();

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

        public async Task<ApiResponse<List<ChatRoom>>> GetAllChatRooms()
        {
            var chatroomList = await _dbcontext.ChatRooms.Include(p => p.Users).Include(p => p.ChatRoomProducts).ThenInclude(p => p.Product).ThenInclude(p => p.ProductInStatuses).OrderByDescending(p => p.ChatRoomId).ToListAsync();
            if (chatroomList == null)
            {
                return new ApiResponse<List<ChatRoom>> { IsSuccess = false, Message = "No chatroom found with that id", StatusCode = 404 };
            }
            return new ApiResponse<List<ChatRoom>> { IsSuccess = true, Message = "Chat room is found", StatusCode = 200, Response = chatroomList };
        }

        public async Task<ApiResponse<Message>> DeleteChatRoomAsync(int ChatRoomId)
        {

            var chatRoom = await _dbcontext.ChatRooms.Include(p => p.ChatRoomProducts).Include(p => p.Users).FirstOrDefaultAsync(p => p.ChatRoomId == ChatRoomId);
            List<int> products = new List<int>();
            foreach (var product in chatRoom.ChatRoomProducts)
            {
                products.Add(product.ProductId);
            }
            foreach (var id in products)
            {
                var product = await _dbcontext.Products.Include(p => p.ProductInStatuses).FirstOrDefaultAsync(p => p.ProductId == id);
                product.ProductInStatuses.Add(new ProductInStatus { ProductStatusId = 1 });
            }
            _dbcontext.ChangeTracker.TrackGraph(chatRoom, entity =>
            {
                if (entity.Entry.State == EntityState.Unchanged)
                {
                    entity.Entry.State = EntityState.Deleted;
                }
            });
            _dbcontext.ChatRooms.Remove(chatRoom);
            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<Message> { IsSuccess = true, Message = "Create message sucessully", StatusCode = 201, Response = new Message() };
            }
            return new ApiResponse<Message> { IsSuccess = false, Message = "Create message failed", StatusCode = 400 };
        }

        public async Task<ApiResponse<ChatRoom>> EditChatRoomAsync(CreateChatRoomModel createChatRoomModel, int chatRoomId)
        {
            //ChatRoom chatRoom = new ChatRoom
            //{
            //    HostUserId = createChatRoomModel.HostUserId,
            //    StartDate = createChatRoomModel.StartDate,
            //    EndDate = createChatRoomModel.StartDate.AddMinutes(5),
            //};
            var chatRoom = await _dbcontext.ChatRooms.Include(p => p.ChatRoomProducts).Include(p => p.Users).FirstOrDefaultAsync(p => p.ChatRoomId == chatRoomId);
            //Edit Products
            List<int> products = new List<int>();
            foreach (var product in chatRoom.ChatRoomProducts)
            {
                products.Add(product.ProductId);
            }
            foreach (var id in products)
            {
                var product = await _dbcontext.Products.Include(p => p.ProductInStatuses).FirstOrDefaultAsync(p => p.ProductId == id);
                product.ProductInStatuses.Add(new ProductInStatus { ProductStatusId = 1 });
            }
            var chatRoomProduct = chatRoom.ChatRoomProducts;
            _dbcontext.ChatRoomProducts.RemoveRange(chatRoomProduct);

            chatRoom.Users = null;
            chatRoom.StartDate = createChatRoomModel.StartDate;
            chatRoom.EndDate = createChatRoomModel.StartDate.AddMinutes(5);

            var rs = await _dbcontext.SaveChangesAsync();
            if (rs > 0)
            {
                return new ApiResponse<ChatRoom> { IsSuccess = true, Message = "Create chat room sucessully", StatusCode = 201, Response = chatRoom };
            }
            return new ApiResponse<ChatRoom> { IsSuccess = false, Message = "Create chat room failed", StatusCode = 400 };
        }

        public async Task<ApiResponse<ChatRoom>> JoinChatRoom(int ChatRoomId, string UserId)
        {
            //var chatRoom = await _dbcontext.ChatRooms.Include(c => c.Users).FirstOrDefaultAsync(c => c.ChatRoomId == ChatRoomId);
            //var user = await _dbcontext.ApplicationUsers.Include(c => c.JoinedChatRooms).FirstOrDefaultAsync(c => c.Id == UserId);

            //if (!chatRoom.Users.Contains(user))
            //{
            //    chatRoom.Users.Add(user);
            //}
            //var rs = await _dbcontext.SaveChangesAsync();
            //if (rs > 0)
            //{
            //    return new ApiResponse<ChatRoom> { IsSuccess = true, Message = "Join chat room sucessully", StatusCode = 201, Response = chatRoom };
            //}
            //return new ApiResponse<ChatRoom> { IsSuccess = false, Message = "Join chat room failed", StatusCode = 400 };
            throw new NotImplementedException();

        }
    }
}
