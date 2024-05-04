using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Models.Authentication.Login
{
    public class LoginModel
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
