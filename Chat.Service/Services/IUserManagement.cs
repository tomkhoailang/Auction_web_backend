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
        public Task<ApiResponse<AuthReponse>> SignIn(SignInModel signInModel);
        public Task<ApiResponse<AuthReponse>> GetJwtTokenAsync(ApplicationUser user);
        public Task<ApiResponse<UserInfoModel>> GetUserInfoAsync(HttpContext httpContext);
        public Task<ApiResponse<ApplicationUser>> GetUserAsync(HttpContext httpContext);


    }
}
