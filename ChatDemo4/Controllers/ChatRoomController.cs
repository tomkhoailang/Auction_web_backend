using Chat.Service.Models;
using Chat.Service.Models.ChatRoom;
using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApiDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetALlChatRoom()
        {
            var getChatRoomRs = await _chatRoomManager.GetAllChatRooms();
            return StatusCode(getChatRoomRs.StatusCode, new { getChatRoomRs.Response });

        }

        [Authorize]
        [HttpGet("{chatRoomId}")]
        public async Task<IActionResult> GetChatRoom(int chatRoomId)
        {
            var getChatRoomRs = await _chatRoomManager.GetChatRoomAsync(chatRoomId);
            return StatusCode(getChatRoomRs.StatusCode, new { getChatRoomRs.Response });

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomModel createChatRoomModel)
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);
            createChatRoomModel.HostUserId = userInfoRes.Response!.Id;
            TimeZoneInfo indochinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Convert startDate to GMT+7
            createChatRoomModel.StartDate = TimeZoneInfo.ConvertTime(createChatRoomModel.StartDate, indochinaTimeZone);
            var apiRs = await _chatRoomManager.CreateChatRoomAsync(createChatRoomModel);
            return StatusCode(apiRs.StatusCode, new { apiRs.Response });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("{chatroomId}/products")]
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

        [HttpDelete("{chatroomId}")]
        public async Task<IActionResult> DeleteChatRoom(int chatroomId)
        {
            var getChatRoomRs = await _chatRoomManager.DeleteChatRoomAsync(chatroomId);
            if (!getChatRoomRs.IsSuccess)
            {
                return StatusCode(getChatRoomRs.StatusCode, new { getChatRoomRs.Message });
            }

            return StatusCode(getChatRoomRs.StatusCode, new { getChatRoomRs.Response });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{chatRoomId}")]
        public async Task<IActionResult> EditChatRoom([FromBody] CreateChatRoomModel createChatRoomModel, int chatRoomId)
        {

            TimeZoneInfo indochinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Convert startDate to GMT+7
            createChatRoomModel.StartDate = TimeZoneInfo.ConvertTime(createChatRoomModel.StartDate, indochinaTimeZone);
            var apiRs = await _chatRoomManager.EditChatRoomAsync(createChatRoomModel, chatRoomId);
            return StatusCode(apiRs.StatusCode, new { apiRs.Response });
        }

        [Authorize]
        [HttpPost("{chatRoomId}/join")]
        public async Task<IActionResult> JoinChatRoom(int chatRoomId)
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);

            var chatRoom = await _chatRoomManager.JoinChatRoom(chatRoomId, userInfoRes.Response!.Id);

            if (!chatRoom.IsSuccess)
            {
                return StatusCode(chatRoom.StatusCode, new { chatRoom.Message });
            }
            return StatusCode(chatRoom.StatusCode, new { chatRoom.Response });
        }
    }
}
