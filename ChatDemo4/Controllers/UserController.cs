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


        

    }
}
