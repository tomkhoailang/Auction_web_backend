using Chat.Data.Data;
using Chat.Data.Models;
using Chat.Service.Models;
using Chat.Service.Models.Authentication;
using Chat.Service.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Chat.Service.Services
{
    public class UserManagement : IUserManagement
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IEmailService _emailService;

        public UserManagement(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext dbcontext, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbcontext = dbcontext;
            _emailService = emailService;
        }

        public async Task<ApiResponse<ApplicationUser>> AssignRole(string role, ApplicationUser user)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return new ApiResponse<ApplicationUser>
                {
                    IsSuccess = false,
                    Message = "Role is not exist",
                    StatusCode = 400,
                };
            }
            var rs = await _userManager.AddToRoleAsync(user, role);
            if (!rs.Succeeded)
            {
                return new ApiResponse<ApplicationUser>
                {
                    IsSuccess = false,
                    Message = "Couldn't create role for the user",
                    StatusCode = 400,
                };
            }
            return new ApiResponse<ApplicationUser>
            {
                IsSuccess = true,
                Message = "Create role successfully",
                StatusCode = 201,
            };
        }

        public async Task<ApiResponse<ApplicationUser>> CreateUserWithTokenAsync(SignUpModel signUpModel)
        {
            var isEmailExists = await _userManager.FindByEmailAsync(signUpModel.Email!);
            if (isEmailExists != null)
            {
                return new ApiResponse<ApplicationUser>
                {
                    IsSuccess = false,
                    Message = "Email is already in use",
                    StatusCode = 400,
                };
            }
            var isUsernameExists = await _userManager.FindByNameAsync(signUpModel.UserName!);
            if (isUsernameExists != null)
            {
                return new ApiResponse<ApplicationUser>
                {
                    IsSuccess = false,
                    Message = "Username is already in use",
                    StatusCode = 400,
                };
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = signUpModel.Email,
                UserName = signUpModel.UserName,
                EmailConfirmed = true

            };
            var result = await _userManager.CreateAsync(user, signUpModel.Password!);
            if (!result.Succeeded)
            {
                return new ApiResponse<ApplicationUser>
                {
                    IsSuccess = false,
                    Message = "Create account failed",
                    StatusCode = 400,
                };

            }
            return new ApiResponse<ApplicationUser>
            {
                IsSuccess = true,
                Message = "Create account successfully",
                StatusCode = 201,
                Response = user
            };

        }
        public async Task<ApiResponse<AuthReponse>> GetOtpByLoginAsync(SignInModel signInModel)
        {
            var user = await _userManager.FindByEmailAsync(signInModel.Email);
            if (user == null)
            {
                return new ApiResponse<AuthReponse> { IsSuccess = false, StatusCode = 404, Message = "The user with that email isn't existed" };
            }
            if (!await _userManager.CheckPasswordAsync(user, signInModel.Password))
            {
                return new ApiResponse<AuthReponse> { IsSuccess = false, StatusCode = 404, Message = "Password is incorrect" };
            }
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user, signInModel.Password, false, true);

            if (user.TwoFactorEnabled)
            {
                var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                var content = "Here is your login OTP: " + otp;
                var message = new MessageEmail(new string[] { user.Email! }, "OTP confirmation", content);
                _emailService.sendEmail(message);
                return new ApiResponse<AuthReponse> { IsSuccess = true, StatusCode = 200, Message = $"We have sent an OTP to your Email {user.Email}" };
            }

            return await GetJwtTokenAsync(user);

        }

        public async Task<ApiResponse<AuthReponse>> GetJwtTokenAsync(ApplicationUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new(ClaimTypes.Role, role));
            }
            var jwtToken = GetToken(authClaims);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(1);
            await _userManager.UpdateAsync(user);
            return new ApiResponse<AuthReponse>
            {
                IsSuccess = true,
                Message = "Token is created successfully",
                StatusCode = 201,
                Response = new AuthReponse
                {
                    AccessToken = new TokenType()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        ExpirationTokenDate = jwtToken.ValidTo
                    },
                    RefreshToken = new TokenType()
                    {
                        Token = user.RefreshToken,
                        ExpirationTokenDate = (DateTime)user.RefreshTokenExpiration,
                    }

                }
            };
        }
        public async Task<ApiResponse<UserInfoModel>> GetUserInfoAsync(HttpContext httpContext)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(httpContext.User.Identity!.Name!);
            if (user == null)
            {
                return new ApiResponse<UserInfoModel> { IsSuccess = false, StatusCode = 401, Message = "Coudn't found the user from the token" };
            }
            return new ApiResponse<UserInfoModel>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "The user is found",
                Response = new UserInfoModel()
                {
                    Id = user.Id,
                    Avatar = user.Avatar,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                    Role = (await _userManager.GetRolesAsync(user))[0],
                }
            };
        }
        public async Task<ApiResponse<ApplicationUser>> GetUserAsync(HttpContext httpContext)
        {
            //ApplicationUser? user = await _userManager.FindByNameAsync(httpContext.User.Identity!.Name!);
            ApplicationUser? user = await _dbcontext.ApplicationUsers.Include(u => u.JoinedChatRooms).FirstOrDefaultAsync(u => u.UserName == httpContext.User.Identity!.Name!);

            if (user == null)
            {
                return new ApiResponse<ApplicationUser> { IsSuccess = false, StatusCode = 401, Message = "NO user found" };
            }
            return new ApiResponse<ApplicationUser>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "The user is found",
                Response = user
            };
        }
        public async Task<ApiResponse<string>> EnableTwoFactorAsync(ApplicationUser user)
        {
            user.TwoFactorEnabled = !user.TwoFactorEnabled;
            var rs = await _userManager.UpdateAsync(user);
            if (!rs.Succeeded)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "Enable 2FA failed" };
            }
            return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Enable 2FA successfully" };
        }

        public async Task<ApiResponse<AuthReponse>> GetJwtTokenFromOtpAsync(string code)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user != null)
            {
                var signIn = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
                if (signIn.Succeeded)
                {
                    return await GetJwtTokenAsync(user);
                }
                return new ApiResponse<AuthReponse> { IsSuccess = false, StatusCode = 404, Message = "OTP is not correct" };
            }
            return new ApiResponse<AuthReponse> { IsSuccess = false, StatusCode = 404, Message = "User cannnot be found" };

        }

        public async Task<ApiResponse<string>> CreateResetPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                var content = $"http://localhost:4200/reset-password?token={token}&email={user.Email}";
                var message = new MessageEmail(new string[] { user.Email! }, "Reset password: ", content);
                _emailService.sendEmail(message);
                return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = $"We have sent a reset password link to your Email {user.Email}" };
            }
            return new ApiResponse<string> { IsSuccess = false, StatusCode = 404, Message = "Something went wrong" };
        }
        public async Task<ApiResponse<string>> VerifyResetPasswordTokenAsync(ApplicationUser user, ResetPasswordModel resetPasswordModel)
        {
            resetPasswordModel.Token = resetPasswordModel.Token.Replace(" ", "+");
            var reset = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.NewPassword);
            if (!reset.Succeeded)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "Token is expired or invalid" };
            }
            return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Password is reset successfully" };
        }
        public async Task<ApiResponse<ApplicationUser>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new ApiResponse<ApplicationUser> { IsSuccess = true, StatusCode = 200, Message = "Get user successfully", Response = user };
            }

            return new ApiResponse<ApplicationUser> { IsSuccess = false, StatusCode = 404, Message = "No user found" };
        }
        public async Task<ApiResponse<AuthReponse>> RenewAccessTokenAsync(AuthReponse authReponse)
        {
            var accessToken = authReponse.AccessToken;
            var refreshToken = authReponse.RefreshToken;
            var principal = GetClaimsPrincipal(accessToken.Token);
            var user = await _userManager.FindByEmailAsync(principal.Identity.Name);
            if (user != null && refreshToken.Token == user.RefreshToken && user.RefreshTokenExpiration > DateTime.Now)
            {
                var resposne = await GetJwtTokenAsync(user);
                return resposne;
            }
            return new ApiResponse<AuthReponse> { IsSuccess = false, StatusCode = 400, Message = "Refresh Token is expired" };
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(ApplicationUser user, ChangePasswordModel changePasswordModel)
        {
            var rs = await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
            if (!rs.Succeeded)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "The current password is incorrect" };
            }
            return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Password is changed successfully" };
        }


        #region PrivateMethods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["ValidIssuer"],
                audience: _configuration["ValidAudience"],
                expires: DateTime.Now.AddMinutes(45),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParams, out SecurityToken securityToken);
            return principal;
        }



        #endregion
    }
}
