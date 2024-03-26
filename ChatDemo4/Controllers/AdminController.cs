using Chat.Service.Models.ChatRoom;
using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApiDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IChatRoomManagement _chatRoomManager;
        private readonly IProductManagement _productManager;
        private readonly IUserManagement _userManager;

        public AdminController(IChatRoomManagement chatRoomManager, IProductManagement productManager, IUserManagement userManager)
        {
            _chatRoomManager = chatRoomManager;
            _productManager = productManager;
            _userManager = userManager;
        }

        


    }
}
