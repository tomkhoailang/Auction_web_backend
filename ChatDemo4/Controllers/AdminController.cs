using Chat.Service.Models.ChatRoom;
using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApiDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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

        [HttpPost("chatroom/create")]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomModel createChatRoomModel)
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);
            createChatRoomModel.HostUserId = userInfoRes.Response!.Id;
            var apiRs = await _chatRoomManager.CreateChatRoomAsync(createChatRoomModel);
            return StatusCode(apiRs.StatusCode, new { apiRs.Response });
        }
        [HttpPost("chatroom/assign")]
        public async Task<IActionResult> AssingProductToChatRoom([FromBody] AssingToChatRoomModel assingToChatRoomModel)
        {
            var getChatRoomRs = await _chatRoomManager.GetChatRoomAsync(assingToChatRoomModel.ChatRoomId);
            if (!getChatRoomRs.IsSuccess)
            {
                return StatusCode(getChatRoomRs.StatusCode, new { getChatRoomRs.Message });
            }
            var getProductsRs = await _productManager.GetMultipleProductsAsync(assingToChatRoomModel.ProductIds);
            if (!getProductsRs.IsSuccess)
            {
                return StatusCode(getProductsRs.StatusCode, new { getProductsRs.Message });
            }
            var assignRs = await _productManager.AssignToChatRoomAsync(getProductsRs.Response!, getChatRoomRs.Response!);

            return StatusCode(assignRs.StatusCode, new { assignRs.Message });
        }

    }
}
