using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Models.Authentication.Login
{
    public class LoginModel
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
