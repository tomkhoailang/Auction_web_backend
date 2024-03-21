using Chat.Service.Models.Authentication;
using Chat.Service.Services;
using Microsoft.AspNetCore.Authorization;
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
            var resSignIn = await _userManager.GetOtpByLoginAsync(signInModel);
            if (!resSignIn.IsSuccess)
            {
                return StatusCode(resSignIn.StatusCode, new { message = resSignIn.Message });
            }
            return StatusCode(resSignIn.StatusCode, new { message = resSignIn.Message, token = resSignIn.Response });
        }
        [HttpPost("enable-2FA")]
        [Authorize]
        public async Task<IActionResult> EnableTwoFactor()
        {
            var userInfoRes = await _userManager.GetUserAsync(HttpContext);
            var rs = await _userManager.EnableTwoFactorAsync(userInfoRes.Response!);
            return StatusCode(rs.StatusCode, new { rs.Message });
        }
        [HttpPost("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code)
        {
            var getOtpLogin = await _userManager.GetJwtTokenFromOtpAsync(code);
            if (!getOtpLogin.IsSuccess)
            {
                return StatusCode(getOtpLogin.StatusCode, new { getOtpLogin.Message });
            }
            return StatusCode(getOtpLogin.StatusCode, new { getOtpLogin.Response });

        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromQuery] string email, [FromBody] ResetPasswordModel resetPasswordModel)
        {
            resetPasswordModel.Email = email;
            resetPasswordModel.Token = token;
            var getUser = await _userManager.GetUserByEmailAsync(email);
            if (!getUser.IsSuccess)
            {
                return StatusCode(getUser.StatusCode, new { getUser.Message });
            }
            var resetPassword = await _userManager.VerifyResetPasswordTokenAsync(getUser.Response, resetPasswordModel);
            return StatusCode(resetPassword.StatusCode, new { resetPassword.Message });

        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string Email)
        {
            var getUser = await _userManager.GetUserByEmailAsync(Email);
            if (!getUser.IsSuccess)
            {
                return StatusCode(getUser.StatusCode, new { getUser.Message });
            }
            var resetPassword = await _userManager.CreateResetPasswordTokenAsync(getUser.Response);
            return StatusCode(resetPassword.StatusCode, new { resetPassword.Message });

        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] AuthReponse authReponse)
        {
            var jwt = await _userManager.RenewAccessTokenAsync(authReponse);
            if (jwt.IsSuccess)
            {
                return Ok(jwt.Response);
            }
            return StatusCode(jwt.StatusCode, new { jwt.Message });

        }
        [HttpPost("change-password")]
        [Authorize]

        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            var userInfoRes = await _userManager.GetUserAsync(HttpContext);

            var changePassword = await _userManager.ChangePasswordAsync(userInfoRes.Response, changePasswordModel);
            return StatusCode(changePassword.StatusCode, new { changePassword.Message });

        }
    }
}
