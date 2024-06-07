using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Services.Models.Authentication.Login
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
    }
}
