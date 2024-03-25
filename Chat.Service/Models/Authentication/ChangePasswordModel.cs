namespace Chat.Service.Models.Authentication
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
