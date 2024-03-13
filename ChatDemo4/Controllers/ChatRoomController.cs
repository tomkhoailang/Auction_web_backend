using Chat.Service.Models;
using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApiDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatRoomController : ControllerBase
    {
        private readonly IChatRoomManagement _chatRoomManager;
        private readonly IUserManagement _userManager;
        private readonly IProductManagement _productManager;

        public ChatRoomController(IChatRoomManagement chatRoomManager, IUserManagement userManager, IProductManagement productManager)
        {
            _chatRoomManager = chatRoomManager;
            _userManager = userManager;
            _productManager = productManager;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetALlUserChatRoom()
        {
            var getUserRs = await _userManager.GetUserAsync(HttpContext);
            var getUserChatRoomRs = await _chatRoomManager.GetUserChatRooms(getUserRs.Response!);
            return StatusCode(getUserChatRoomRs.StatusCode, new { getUserChatRoomRs.Response });

        }
        [Authorize]
        [HttpGet("{chatRoomId}/products")]
        public async Task<IActionResult> getProductListFromChatRoom(int chatRoomId)
        {
            var getProductList = await _productManager.GetProductListFromChatRoomAsync(chatRoomId);
            if (!getProductList.IsSuccess)
            {
                return StatusCode(getProductList.StatusCode, new { getProductList.Message });
            }
            return StatusCode(getProductList.StatusCode, new { getProductList.Response });
        }
        [Authorize]
        [HttpPost("{chatRoomId}/messages")]
        public async Task<IActionResult> createMessage(int chatRoomId, [FromBody] CreateMessageModel createMessageModel)
        {
            createMessageModel.ChatRoomId = chatRoomId;
            var getUser = await _userManager.GetUserAsync(HttpContext);
            createMessageModel.SenderId = getUser.Response!.Id;
            var createMessage = await _chatRoomManager.CreateMessageAsync(createMessageModel);
            if (!createMessage.IsSuccess)
            {
                return StatusCode(createMessage.StatusCode, new { createMessage.Message });
            }
            return StatusCode(createMessage.StatusCode, new { createMessage.Response });
            throw new NotImplementedException();
        }
    }
}
