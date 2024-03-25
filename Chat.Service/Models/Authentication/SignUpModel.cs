using System.ComponentModel.DataAnnotations;

namespace Chat.Service.Models.Authentication
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Must have an email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Must have an username")]

        public string? UserName { get; set; }
        [Required(ErrorMessage = "Must have a password")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Must have a role")]
        public string? Role { get; set; }
    }
}
