using Chat.Service.Models.Authentication;
using Chat.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApiDemo4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserManagement _userManager;

        public AuthenticationController(IUserManagement userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            var resSignUp = await _userManager.CreateUserWithTokenAsync(signUpModel);
            if (!resSignUp.IsSuccess)
            {
                return StatusCode(resSignUp.StatusCode, new { message = resSignUp.Message });
            }
            var resAssignRole = await _userManager.AssignRole(signUpModel.Role!, resSignUp.Response!);
            if (!resAssignRole.IsSuccess)
            {
                return StatusCode(resAssignRole.StatusCode, new { message = resSignUp.Message });
            }
            return StatusCode(resSignUp.StatusCode, new { message = resSignUp.Message });

        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            var resSignIn = await _userManager.SignIn(signInModel);
            if (!resSignIn.IsSuccess)
            {
                return StatusCode(resSignIn.StatusCode, new { message = resSignIn.Message });
            }
            return StatusCode(resSignIn.StatusCode, new { message = resSignIn.Message, token = resSignIn.Response });
        }
    }
}
