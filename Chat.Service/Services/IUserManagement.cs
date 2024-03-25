using Chat.Data.Models;
using Chat.Service.Models;
using Chat.Service.Models.Authentication;
using Chat.Service.Models.User;
using Microsoft.AspNetCore.Http;

namespace Chat.Service.Services
{
    public interface IUserManagement
    {
        public Task<ApiResponse<ApplicationUser>> CreateUserWithTokenAsync(SignUpModel signUpModel);
        public Task<ApiResponse<ApplicationUser>> AssignRole(string role, ApplicationUser user);
        public Task<ApiResponse<AuthReponse>> GetOtpByLoginAsync(SignInModel signInModel);
        public Task<ApiResponse<AuthReponse>> GetJwtTokenAsync(ApplicationUser user);
        public Task<ApiResponse<string>> EnableTwoFactorAsync(ApplicationUser user);
        public Task<ApiResponse<UserInfoModel>> GetUserInfoAsync(HttpContext httpContext);
        public Task<ApiResponse<ApplicationUser>> GetUserByEmailAsync(string email);
        public Task<ApiResponse<string>> CreateResetPasswordTokenAsync(ApplicationUser user);
        public Task<ApiResponse<string>> VerifyResetPasswordTokenAsync(ApplicationUser user, ResetPasswordModel resetPasswordModel);

        public Task<ApiResponse<ApplicationUser>> GetUserAsync(HttpContext httpContext);
        public Task<ApiResponse<AuthReponse>> GetJwtTokenFromOtpAsync(string code);
        public Task<ApiResponse<AuthReponse>> RenewAccessTokenAsync(AuthReponse authReponse);
        public Task<ApiResponse<string>> ChangePasswordAsync(ApplicationUser user, ChangePasswordModel changePasswordModel);





    }
}
