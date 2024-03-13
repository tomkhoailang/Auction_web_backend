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

        public UserManagement(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext dbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbcontext = dbcontext;
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
                UserName = signUpModel.UserName
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
        public async Task<ApiResponse<AuthReponse>> SignIn(SignInModel signInModel)
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

        #region PrivateMethods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var a = _configuration;
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["ValidIssuer"],
                audience: _configuration["ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        #endregion
    }
}
