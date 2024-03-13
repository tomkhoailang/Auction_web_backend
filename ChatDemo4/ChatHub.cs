using Chat.Service.Services;
using ChatApiDemo4.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatDemo4
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, UserRoomConnection> _connection;
        private readonly IProductManagement _producManager;

        public ChatHub(IDictionary<string, UserRoomConnection> connection, IProductManagement producManager)
        {
            _connection = connection;
            _producManager = producManager;
        }

        public async Task JoinRoom(UserRoomConnection userRoomConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userRoomConnection.Room!);
            _connection[Context.ConnectionId] = userRoomConnection;
            await Clients.Group(userRoomConnection.Room!).SendAsync("ReceiveMessage:", "Bot", $"{userRoomConnection.User} has joined the group", DateTime.Now);
            await SendConnectedUser(userRoomConnection.Room!);
        }
        public async Task SendBidding(decimal biddingAmount)
        {
            if (_connection.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection))
            {
                await Clients.Group(userRoomConnection.Room!).SendAsync("BiddingAmount:", userRoomConnection.User, biddingAmount, DateTime.Now);
            }
        }
        public async Task SendMessage(string message)
        {
            if (_connection.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection))
            {
                await Clients.Group(userRoomConnection.Room!).SendAsync("ReceiveMessage:", userRoomConnection.User, message, DateTime.Now);
            }
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (!_connection.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection))
            {
                return base.OnDisconnectedAsync(exception);
            }
            _connection.Remove(Context.ConnectionId);
            Clients.Group(userRoomConnection.Room!).SendAsync("ReceiveMessage:", "Bot", $"{userRoomConnection.User} has left the group", DateTime.Now);
            SendConnectedUser(userRoomConnection.Room!);
            return base.OnDisconnectedAsync(exception);
        }
        public Task SendConnectedUser(string room)
        {
            var users = _connection.Values.Where(u => u.Room == room).Select(s => s.User);
            return Clients.Group(room).SendAsync("ConnectedUser", users);
        }
    }
}
