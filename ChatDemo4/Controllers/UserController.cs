using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApiDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IProductManagement _productManager;
        private readonly IUserManagement _userManager;

        public UserController(IProductManagement productManager, IUserManagement userManager)
        {
            _productManager = productManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);
            return StatusCode(userInfoRes.StatusCode, new { userInfoRes.Response });
        }


        [HttpGet("products")]
        public async Task<IActionResult> getProductListFromUser()
        {
            //var userInfoRes = await _userManager.GetUserInfoAsync(HttpContext);

            var listProduct = await _productManager.GetProductFromUserAsync("a3a57a6f-c8d3-46f8-adb2-ea3361dd719a");
            if (!listProduct.IsSuccess)
            {
                return StatusCode(listProduct.StatusCode, new { listProduct.Message });
            }
            return StatusCode(listProduct.StatusCode, new { listProduct.Response });
        }

    }
}
