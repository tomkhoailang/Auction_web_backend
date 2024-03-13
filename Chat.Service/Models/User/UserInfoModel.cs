namespace Chat.Service.Models.User
{
    public class UserInfoModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool TwoFactorEnabled { get; set; }
        public string Avatar { get; set; } = null!;
        public string Role { get; set; } = null!;

    }
}
